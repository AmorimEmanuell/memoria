using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [SerializeField] private PotionPickup potionPrefab = default;

    private const int MaxActivePotions = 3;
    private readonly List<PotionPickup> potions = new List<PotionPickup>(MaxActivePotions);
    private int activePotions;

#if D
    private const float PotionDroppingChance = 1f;
#else
    private const float PotionDroppingChance = 0.1f;
#endif

    private void Awake()
    {
        for (var i = 0; i < MaxActivePotions; i++)
        {
            var potion = Instantiate(potionPrefab, transform);
            potion.gameObject.SetActive(false);
            potion.OnPickup += Potion_OnPickup;
            potion.OnFadeRoutineComplete += Potion_OnFadeRoutineComplete;

            potions.Add(potion);
        }

        Events.instance.AddListener<EnemyDefeatEvent>(OnEnemyDefeated);
    }

    private void OnDestroy()
    {
        for (var i = 0; i < MaxActivePotions; i++)
        {
            potions[i].OnPickup -= Potion_OnPickup;
            potions[i].OnFadeRoutineComplete -= Potion_OnFadeRoutineComplete;
        }

        Events.instance.RemoveListener<EnemyDefeatEvent>(OnEnemyDefeated);
    }

    private void Potion_OnPickup(PotionPickup potion)
    {
        DeactivatePotion(potion);
        Events.instance.Raise(new PotionPickedEvent());
    }

    private void Potion_OnFadeRoutineComplete(PotionPickup potion)
    {
        DeactivatePotion(potion);
    }

    private void DeactivatePotion(PotionPickup potion)
    {
        potion.ResetState();
        potion.gameObject.SetActive(false);
        potions.Remove(potion);
        potions.Add(potion);
        activePotions--;
    }

    private void OnEnemyDefeated(EnemyDefeatEvent e)
    {
        var potion = SelectAvailablePotion();
        potion.transform.position = e.EnemyController.ModelPosition;
        potion.gameObject.SetActive(true);

        var direction = CalculateMoveDirection();
        potion.transform.DOMove(e.EnemyController.ModelPosition + direction, 0.5f).OnComplete(() =>
        {
            potion.PrepareToFade();
        });
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
        var angle = UnityEngine.Random.Range(Mathf.PI, 2 * Mathf.PI);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
