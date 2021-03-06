﻿using DG.Tweening;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerHUDController hudController = default;
    [SerializeField] private new Camera camera = default;

    private int
        currentHealth,
        currentPotions;

    private float HealthPercentage => (float)currentHealth / Data.MaxHealth;

    public PlayerSaveData Data { get; private set; }
    public bool IsAlive => currentHealth > 0;
    public int CurrentScore { get; private set; }

    private void Awake()
    {
        Data = PlayerSaveData.Load();

        hudController.OnPotionButtonClicked += PotionButton_Clicked;
        Events.instance.AddListener<PotionPickedEvent>(OnPotionPickedup);
    }

    private void Start()
    {
        InitializeState();
    }

    private void OnDestroy()
    {
        hudController.OnPotionButtonClicked -= PotionButton_Clicked;
        Events.instance.RemoveListener<PotionPickedEvent>(OnPotionPickedup);
    }

    public void InitializeState()
    {
        currentHealth = Data.MaxHealth;
        currentPotions = Data.MaxPotions;
        CurrentScore = 0;

        hudController.ActivateScore(true);
        hudController.SetInitialValues(Data);
    }

    private void PotionButton_Clicked()
    {
        AddPotions(-1);
        AddHealth(Data.PotionStrength);

        hudController.ActivatePotionButton(ShouldActivatePotionButton());
    }

    private bool ShouldActivatePotionButton()
    {
        return
            IsAlive &&
            currentHealth < Data.MaxHealth &&
            currentPotions > 0;
    }

    public void ApplyDamage(int damageReceived)
    {
        AddHealth(-damageReceived);

        hudController.ActivatePotionButton(ShouldActivatePotionButton());

        camera.DOShakePosition(.2f, .05f);
    }

    private void OnPotionPickedup(PotionPickedEvent e)
    {
        if (currentPotions < Data.MaxPotions)
        {
            AddPotions(1);
        }
        else
        {
            if (currentHealth < Data.MaxHealth)
            {
                AddHealth(Data.PotionStrength);
            }
            else
            {
                AddScore(Data.PotionStrength);
            }
        }

        hudController.ActivatePotionButton(ShouldActivatePotionButton());
    }

    public void AddScore(int scoreGained)
    {
        CurrentScore += scoreGained;
        hudController.UpdateScore(CurrentScore);
    }

    private void AddHealth(int healthGained)
    {
        currentHealth += healthGained;
        currentHealth = Mathf.Clamp(currentHealth, 0, Data.MaxHealth);
        hudController.UpdateHealth(currentHealth, HealthPercentage);
    }

    private void AddPotions(int potionsGained)
    {
        currentPotions += potionsGained;
        currentPotions = Mathf.Clamp(currentPotions, 0, Data.MaxPotions);
        hudController.UpdatePotionCount(currentPotions);
    }

    public void ActivateScore(bool active)
    {
        hudController.ActivateScore(active);
    }
}
