using DG.Tweening;
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

    private int _currentHealth, _maxHealth;

    public EnemyData Data { get; private set; }
    public bool IsAlive => _currentHealth > 0;

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

    public void Damage(int damageDealt)
    {
        _currentHealth -= damageDealt;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        var healthPercent = (float)_currentHealth / _maxHealth;

        _healthSlider.DOValue(_currentHealth, 0.5f);
        _healthFill.DOColor(_colorGradient.Evaluate(healthPercent), 0.5f);

        _healthText.text = Mathf.Clamp(_currentHealth, 0, _maxHealth).ToString();
    }
}
