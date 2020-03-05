using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _modelContainer = default;

    [Header("HUD")]
    [SerializeField] private Slider _healthSlider = default;
    [SerializeField] private TextMeshProUGUI _healthText = default;
    [SerializeField] private Image _healthFill = default;
    [SerializeField] private Gradient _colorGradient = default;

    public Action<int> OnAttackAnimationFinished;
    public Action<bool> OnDamageAnimationFinished;

    public int TotalHealth { get; private set; }
    public Vector2Int GridSize { get; private set; }
    public int TurnsToAttack { get; private set; }
    public int AttackPower { get; private set; }

    private const float HealthAnimDuration = 0.5f;

    private const string
        GetHitTrigger = "GetHit",
        RemainingTurnsToAttackInt = "RemainingTurnsToAttack",
        HealthInt = "Health";

    private AnimatorController _animator;

    private int
        _currentHealth,
        _remainingTurnsToAttack;

    private int RemainingTurnsToAttack
    {
        get { return _remainingTurnsToAttack; }
        set
        {
            _remainingTurnsToAttack = value;
            _animator.SetInteger(RemainingTurnsToAttackInt, _remainingTurnsToAttack);
        }
    }

    private void OnDestroy()
    {
        _animator.OnDamageAnimationFinished -= FinishDamageAnimation;
        _animator.OnAttackAnimationFinished -= FinishAttackAnimation;
    }

    public void InitializeProperties(int totalHealth, Vector2Int gridSize, int turnsToAttack, int attackPower, GameObject model)
    {
        AttachModelToContainer(model);

        TotalHealth = totalHealth;
        GridSize = gridSize;
        TurnsToAttack = turnsToAttack;
        AttackPower = attackPower;

        _healthSlider.maxValue = TotalHealth;
    }

    private void AttachModelToContainer(GameObject model)
    {
        model.transform.SetParent(_modelContainer);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        _animator = model.AddComponent<AnimatorController>();

        _animator.OnDamageAnimationFinished += FinishDamageAnimation;
        _animator.OnAttackAnimationFinished += FinishAttackAnimation;
    }

    public void ResetDefaultProperties()
    {
        _currentHealth = TotalHealth;

        _healthSlider.value = _currentHealth;
        _healthFill.color = _colorGradient.Evaluate(1);
        _healthText.text = _currentHealth.ToString();

        RemainingTurnsToAttack = TurnsToAttack;
    }

    public bool Damage(int damageReceived)
    {
        _currentHealth -= damageReceived;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, TotalHealth);

        _animator.SetInteger(HealthInt, _currentHealth);
        _animator.SetTrigger(GetHitTrigger);

        var healthPercent = (float)_currentHealth / TotalHealth;
        _healthFill.DOColor(_colorGradient.Evaluate(healthPercent), HealthAnimDuration);
        _healthSlider.DOValue(_currentHealth, HealthAnimDuration);
        _healthText.text = _currentHealth.ToString();

        return _currentHealth > 0;
    }

    public void CheckIfShouldAttack()
    {
        RemainingTurnsToAttack--;
    }

    private void FinishDamageAnimation()
    {
        OnDamageAnimationFinished?.Invoke(_currentHealth > 0);
    }

    private void FinishAttackAnimation()
    {
        RemainingTurnsToAttack = TurnsToAttack;
        OnAttackAnimationFinished?.Invoke(AttackPower);
    }
}
