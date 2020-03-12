using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerHUDController _hudController = default;
    [SerializeField] private Camera _camera = default;

    private int _currentHealth, _currentPotions;
    private float HealthPercentage => (float)_currentHealth / Data.MaxHealth;

    public PlayerSaveData Data { get; private set; }

    private void Awake()
    {
        Data = PlayerSaveData.Load();
        _currentHealth = Data.MaxHealth;
        _currentPotions = Data.MaxPotions;

        _hudController.OnPotionButtonClicked += PotionButton_Clicked;
    }

    private void Start()
    {
        ResetDefaultValues();
    }

    private void OnDestroy()
    {
        _hudController.OnPotionButtonClicked -= PotionButton_Clicked;
    }

    public void ResetDefaultValues()
    {
        _hudController.SetInitialValues(this);
    }

    private void PotionButton_Clicked()
    {
        _currentPotions--;

        _currentHealth += Data.PotionStrength;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, Data.MaxHealth);

        _hudController.UpdatePotions(_currentPotions);
        _hudController.UpdateHealth(_currentHealth, HealthPercentage);
        _hudController.ActivatePotionButton(ShouldActivatePotionButton());
    }

    private bool ShouldActivatePotionButton()
    {
        return
            _currentHealth > 0 &&
            _currentHealth < Data.MaxHealth &&
            _currentPotions > 0;
    }

    public bool ApplyDamage(int damageReceived)
    {
        _currentHealth -= damageReceived;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, Data.MaxHealth);

        _hudController.UpdateHealth(_currentHealth, HealthPercentage);
        _hudController.ActivatePotionButton(ShouldActivatePotionButton());

        _camera.DOShakePosition(.2f, .05f);

        return _currentHealth > 0;
    }
}
