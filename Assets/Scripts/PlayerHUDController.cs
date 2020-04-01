using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Button _potionBtn = default;
    [SerializeField] private TextMeshProUGUI _potionCount = default;
    [SerializeField] private Slider _healthSlider = default;
    [SerializeField] private Image _healthFill = default;
    [SerializeField] private Gradient _healthColorGradient = default;

    private const float AnimationTime = 0.5f;

    public Action OnPotionButtonClicked;

    private void Awake()
    {
        _potionBtn.onClick.AddListener(() => OnPotionButtonClicked?.Invoke());
    }

    private void OnDestroy()
    {
        _potionBtn.onClick.RemoveAllListeners();
    }

    public void SetInitialValues(PlayerStatus player)
    {
        _healthSlider.maxValue = player.Data.MaxHealth;
        _healthSlider.value = player.Data.MaxHealth;

        _healthFill.color = _healthColorGradient.Evaluate(1);

        _potionCount.text = player.Data.MaxPotions.ToString();
        _potionBtn.interactable = false;
    }

    public void UpdateHealth(int currentHealth, float healthPercentage)
    {
        _healthSlider.DOValue(currentHealth, AnimationTime);

        var healthColor = _healthColorGradient.Evaluate(healthPercentage);
        _healthFill.DOColor(healthColor, AnimationTime);
    }

    public void UpdatePotions(int currentPotions)
    {
        _potionCount.text = currentPotions.ToString();
    }

    public void ActivatePotionButton(bool active)
    {
        _potionBtn.interactable = active;
    }

    public void UpdateScore(int currentScore)
    {
        Debug.Log("Score: " + currentScore);
    }
}
