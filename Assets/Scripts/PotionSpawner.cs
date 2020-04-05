using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [SerializeField] private PotionPickup potionPrefab = default;

    private const int MaxActivePotions = 3;

    private readonly List<PotionPickup> potions = new List<PotionPickup>(MaxActivePotions);
    private int activePotions;

    private void Awake()
    {
        for (var i = 0; i < MaxActivePotions; i++)
        {
            var potion = Instantiate(potionPrefab, transform);
            potion.OnPickup += Potion_OnPickup;
            potion.gameObject.SetActive(false);
            potions.Add(potion);
        }
    }

    private void OnDestroy()
    {
        for (var i = 0; i < MaxActivePotions; i++)
        {
            potions[i].OnPickup -= Potion_OnPickup;
        }
    }

    private void Potion_OnPickup(PotionPickup potion)
    {
        potion.gameObject.SetActive(false);
        potions.Remove(potion);
        potions.Add(potion);
        activePotions--;

        //TODO: Notify player
    }

    public void Spawn(Vector3 location)
    {
        var potion = SelectAvailablePotion();
        potion.transform.position = location;
        potion.gameObject.SetActive(true);

        var direction = CalculateMoveDirection();
        potion.transform.DOMove(location + direction, 0.5f);
    }

    private PotionPickup SelectAvailablePotion()
    {
        PotionPickup potion;

        if (activePotions == MaxActivePotions)
        {
            potion = potions[0];
            potions.RemoveAt(0);
            potions.Add(potion);
        }
        else
        {
            potion = potions[activePotions];
            activePotions++;
        }

        return potion;
    }

    private Vector3 CalculateMoveDirection()
    {
        var angle = UnityEngine.Random.value * (Mathf.PI * 2);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
