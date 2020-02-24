using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusController : MonoBehaviour
{
    [SerializeField] private Button _potionBtn;
    [SerializeField] private TextMeshProUGUI _potionCountText;
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private Image _playerHealthFill;
    [SerializeField] private Gradient _colorGradient = default;

    private const int PlayerMaxHealth = 3, PotionMaxCount = 3;
    private const float HealthAnimDuration = 0.5f;

    private int _playerHealth, _potionCount;

    private int PlayerHealth
    {
        get { return _playerHealth; }
        set
        {
            _playerHealth = Mathf.Clamp(value, 0, PlayerMaxHealth);
            _potionBtn.interactable = _playerHealth > 0 && _playerHealth < PlayerMaxHealth && _potionCount > 0;
        }
    }

    private int PotionCount
    {
        get { return _potionCount; }
        set
        {
            _potionCount = Mathf.Clamp(value, 0, PotionMaxCount);
            _potionCountText.text = _potionCount.ToString();
            _potionBtn.interactable = _playerHealth > 0 && _playerHealth < PlayerMaxHealth && _potionCount > 0;
        }
    }

    private void Awake()
    {
        _potionBtn.onClick.AddListener(OnPotionBtnClicked);
    }

    private void Start()
    {
        SetDefaultValues();
    }

    private void OnDestroy()
    {
        _potionBtn.onClick.RemoveAllListeners();
    }

    public void SetDefaultValues()
    {
        PlayerHealth = PlayerMaxHealth;
        PotionCount = PotionMaxCount;

        _playerHealthSlider.maxValue = PlayerMaxHealth;
        _playerHealthSlider.value = PlayerMaxHealth;
        _playerHealthFill.color = _colorGradient.Evaluate(1);
    }

    private void OnPotionBtnClicked()
    {
        PotionCount--;
        AddToHealth(1);
    }

    private void AddToHealth(int amount)
    {
        PlayerHealth += amount;

        _playerHealthSlider.DOValue(_playerHealth, HealthAnimDuration);

        var healthPercent = (float)_playerHealth / PlayerMaxHealth;
        _playerHealthFill.DOColor(_colorGradient.Evaluate(healthPercent), HealthAnimDuration);
    }

    public bool ReduceHealth(int amount)
    {
        AddToHealth(-amount);
        return PlayerHealth > 0;
    }
}
