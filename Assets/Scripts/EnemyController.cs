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

    public Action<int> OnAttackAnimationFinished;
    public Action<bool> OnDamageAnimationFinished;

    public EnemyData Data { get; private set; }

    private const float HealthAnimDuration = 0.5f;
    private const string
        GetHitTrigger = "GetHit",
        RemainingTurnsToAttackInt = "RemainingTurnsToAttack",
        HealthInt = "Health";

    private int _currentHealth, _maxHealth, _remainingTurnsToAttack;
    private int RemainingTurnsToAttack
    {
        get { return _remainingTurnsToAttack; }
        set
        {
            _remainingTurnsToAttack = value;
            _animator.SetInteger(RemainingTurnsToAttackInt, _remainingTurnsToAttack);
        }
    }

    private void Awake()
    {
        _animator.OnDamageAnimationFinished += () => OnDamageAnimationFinished?.Invoke(_currentHealth > 0);
        _animator.OnAttackAnimationFinished += FinishAttack;
    }

    private void OnDestroy()
    {
        _animator.OnDamageAnimationFinished -= () => OnDamageAnimationFinished?.Invoke(_currentHealth > 0);
        _animator.OnAttackAnimationFinished -= FinishAttack;
    }

    public void SetData(EnemyData enemyData)
    {
        Data = enemyData;

        _maxHealth = Data.Health;
        _healthSlider.maxValue = _maxHealth;
        RemainingTurnsToAttack = Data.TurnsToAttack;

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
        RemainingTurnsToAttack--;
    }

    private void FinishAttack()
    {
        RemainingTurnsToAttack = Data.TurnsToAttack;
        OnAttackAnimationFinished?.Invoke(Data.AttackPower);
    }
}
