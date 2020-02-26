using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AnimatorController _animator = default;
    [SerializeField] private Slider _healthSlider = default;
    [SerializeField] private TextMeshProUGUI _healthText = default;
    [SerializeField] private Image _healthFill = default;
    [SerializeField] private Gradient _colorGradient = default;

    public Action<int> OnAttack;
    public Action<bool> OnDamageAnimationFinished;

    public EnemyData Data { get; private set; }

    private const float HealthAnimDuration = 0.5f;
    private const string
        GetHitTrigger = "GetHit",
        AttackTrigger = "Attack",
        PreparingAttackStageInt = "PreparingAttackStage",
        HealthInt = "Health";

    private int _currentHealth, _maxHealth, _remainingTurnsToAttack;

    private void Awake()
    {
        _animator.OnDamageAnimationFinished += () => OnDamageAnimationFinished?.Invoke(_currentHealth > 0);
    }

    private void OnDestroy()
    {
        _animator.OnDamageAnimationFinished -= () => OnDamageAnimationFinished?.Invoke(_currentHealth > 0);
    }

    public void SetData(EnemyData enemyData)
    {
        Data = enemyData;

        _maxHealth = Data.Health;
        _healthSlider.maxValue = _maxHealth;
        _remainingTurnsToAttack = Data.TurnsToAttack;

        ResetVisuals();
    }

    private void ResetVisuals()
    {
        _healthSlider.value = _maxHealth;
        _healthFill.color = _colorGradient.Evaluate(1);
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString();
    }

    public bool Damage(int damageDealt)
    {
        _currentHealth -= damageDealt;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        _animator.SetInteger(HealthInt, _currentHealth);
        _animator.SetTrigger(GetHitTrigger);

        var healthPercent = (float)_currentHealth / _maxHealth;
        _healthFill.DOColor(_colorGradient.Evaluate(healthPercent), HealthAnimDuration);
        _healthSlider.DOValue(_currentHealth, HealthAnimDuration);
        _healthText.text = _currentHealth.ToString();

        return _currentHealth > 0;
    }

    public void CheckIfShouldAttack()
    {
        if (--_remainingTurnsToAttack == 0)
        {
            _remainingTurnsToAttack = Data.TurnsToAttack;
            OnAttack?.Invoke(Data.AttackPower);
        }
    }
}
