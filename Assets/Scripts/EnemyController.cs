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
    [SerializeField] private EnemyHUDController _hudController = default;

    private const string
        GetHitTrigger = "GetHit",
        RemainingTurnsToAttackInt = "RemainingTurnsToAttack",
        HealthInt = "Health";

    private EnemyAnimatorController _animator;
    private int _currentHealth, _remainingTurnsToAttack;
    private float HealthPercentage => (float)_currentHealth / Data.MaxHealth;

    public Action<int> OnAttackAnimationFinished;
    public Action<bool> OnDamageAnimationFinished;

    public EnemyData Data { get; private set; }

    private void OnDestroy()
    {
        _animator.OnDamageAnimationFinished -= FinishDamageAnimation;
        _animator.OnAttackAnimationFinished -= FinishAttackAnimation;
    }

    public void SetData(EnemyData data)
    {
        Data = data;
        LoadModelInContainer(data.ModelPrefab);
        ResetDefaultProperties();
    }

    private void LoadModelInContainer(GameObject modelPrefab)
    {
        var model = Instantiate(modelPrefab, _modelContainer);

        _animator = model.AddComponent<EnemyAnimatorController>();
        _animator.OnDamageAnimationFinished += FinishDamageAnimation;
        _animator.OnAttackAnimationFinished += FinishAttackAnimation;
    }

    public void ResetDefaultProperties()
    {
        _currentHealth = Data.MaxHealth;
        _hudController.SetInitialValues(Data.MaxHealth);

        ResetRemainingTurnsToAttack();
    }

    public bool ApplyDamage(int damageReceived)
    {
        _currentHealth -= damageReceived;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, Data.MaxHealth);

        _animator.SetInteger(HealthInt, _currentHealth);
        _animator.SetTrigger(GetHitTrigger);

        _hudController.UpdateHealth(_currentHealth, HealthPercentage);

        return _currentHealth > 0;
    }

    public void CheckIfShouldAttack()
    {
        _remainingTurnsToAttack--;
        _animator.SetInteger(RemainingTurnsToAttackInt, _remainingTurnsToAttack);
    }

    private void ResetRemainingTurnsToAttack()
    {
        _remainingTurnsToAttack = Data.TurnsToAttack;
        _animator.SetInteger(RemainingTurnsToAttackInt, Data.TurnsToAttack);
    }

    private void FinishDamageAnimation()
    {
        OnDamageAnimationFinished?.Invoke(_currentHealth > 0);
    }

    private void FinishAttackAnimation()
    {
        ResetRemainingTurnsToAttack();
        OnAttackAnimationFinished?.Invoke(Data.AttackPower);
    }
}
