using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider = default;
    [SerializeField] private TextMeshProUGUI _healthText = default;
    [SerializeField] private Image _healthFill = default;
    [SerializeField] private Gradient _colorGradient = default;

    public EnemyData Data { get; private set; }

    private const float HealthAnimDuration = 0.5f;

    private int _currentHealth, _maxHealth;
    private Tweener _healthTweener;

    public void SetData(EnemyData enemyData)
    {
        Data = enemyData;

        _maxHealth = Data.Health;
        _healthSlider.maxValue = _maxHealth;

        ResetVisuals();
    }

    public void ResetVisuals()
    {
        _healthSlider.value = _maxHealth;
        _healthFill.color = _colorGradient.Evaluate(1);
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString();
    }

    public bool Damage(int damageDealt, Action<bool> onHealthAnimComplete)
    {
        _currentHealth -= damageDealt;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        var healthPercent = (float)_currentHealth / _maxHealth;

        _healthFill.DOColor(_colorGradient.Evaluate(healthPercent), HealthAnimDuration);

        if (_healthTweener != null)
        {
            _healthTweener.Kill();
        }

        _healthTweener = _healthSlider.DOValue(_currentHealth, HealthAnimDuration);
        _healthTweener.OnComplete(() => onHealthAnimComplete(_currentHealth > 0));

        _healthText.text = Mathf.Clamp(_currentHealth, 0, _maxHealth).ToString();

        return _currentHealth > 0;
    }
}
