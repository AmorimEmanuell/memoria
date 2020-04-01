using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerHUDController hudController = default;
    [SerializeField] private new Camera camera = default;

    private int
        currentHealth,
        currentPotions,
        currentScore;

    private float HealthPercentage => (float)currentHealth / Data.MaxHealth;

    public PlayerSaveData Data { get; private set; }

    private void Awake()
    {
        Data = PlayerSaveData.Load();

        hudController.OnPotionButtonClicked += PotionButton_Clicked;
    }

    private void Start()
    {
        ResetDefaultValues();
    }

    private void OnDestroy()
    {
        hudController.OnPotionButtonClicked -= PotionButton_Clicked;
    }

    public void ResetDefaultValues()
    {
        currentHealth = Data.MaxHealth;
        currentPotions = Data.MaxPotions;
        currentScore = 0;

        hudController.SetInitialValues(Data);
    }

    private void PotionButton_Clicked()
    {
        currentPotions--;

        currentHealth += Data.PotionStrength;
        currentHealth = Mathf.Clamp(currentHealth, 0, Data.MaxHealth);

        hudController.UpdatePotions(currentPotions);
        hudController.UpdateHealth(currentHealth, HealthPercentage);
        hudController.ActivatePotionButton(ShouldActivatePotionButton());
    }

    private bool ShouldActivatePotionButton()
    {
        return
            currentHealth > 0 &&
            currentHealth < Data.MaxHealth &&
            currentPotions > 0;
    }

    public bool ApplyDamage(int damageReceived)
    {
        currentHealth -= damageReceived;
        currentHealth = Mathf.Clamp(currentHealth, 0, Data.MaxHealth);

        hudController.UpdateHealth(currentHealth, HealthPercentage);
        hudController.ActivatePotionButton(ShouldActivatePotionButton());

        camera.DOShakePosition(.2f, .05f);

        return currentHealth > 0;
    }

    public void AddToScore(int score)
    {
        currentScore += score;
        hudController.UpdateScore(currentScore);
    }
}
