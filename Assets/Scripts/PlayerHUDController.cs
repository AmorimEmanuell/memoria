using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Button potionBtn = default;
    [SerializeField] private TextMeshProUGUI potionCount = default;
    [SerializeField] private Slider healthSlider = default;
    [SerializeField] private Image healthFill = default;
    [SerializeField] private Gradient healthColorGradient = default;

    private const float AnimationTime = 0.5f;

    public Action OnPotionButtonClicked;

    private void Awake()
    {
        potionBtn.onClick.AddListener(() => OnPotionButtonClicked?.Invoke());
    }

    private void OnDestroy()
    {
        potionBtn.onClick.RemoveAllListeners();
    }

    public void SetInitialValues(PlayerSaveData playerData)
    {
        healthSlider.maxValue = playerData.MaxHealth;
        healthSlider.value = playerData.MaxHealth;

        healthFill.color = healthColorGradient.Evaluate(1);

        potionCount.text = playerData.MaxPotions.ToString();
        potionBtn.interactable = false;

        //TODO: Reset Score text count
    }

    public void UpdateHealth(int currentHealth, float healthPercentage)
    {
        healthSlider.DOValue(currentHealth, AnimationTime);

        var healthColor = healthColorGradient.Evaluate(healthPercentage);
        healthFill.DOColor(healthColor, AnimationTime);
    }

    public void UpdatePotions(int currentPotions)
    {
        potionCount.text = currentPotions.ToString();
    }

    public void ActivatePotionButton(bool active)
    {
        potionBtn.interactable = active;
    }

    public void UpdateScore(int currentScore)
    {
        Debug.Log("Score: " + currentScore);
        //TODO:Create score text count
    }
}
