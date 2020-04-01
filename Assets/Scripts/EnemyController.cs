using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform modelContainer = default;
    [SerializeField] private EnemyHUDController hudController = default;

    private const string
        GetHitTrigger = "GetHit",
        RemainingTurnsToAttackInt = "RemainingTurnsToAttack",
        HealthInt = "Health";

    private EnemyAnimatorController animator;
    private int currentHealth, remainingTurnsToAttack;
    private float HealthPercentage => (float)currentHealth / Data.MaxHealth;

    public Action<int> OnAttackAnimationFinished;
    public Action<bool> OnDamageAnimationFinished;

    public EnemyData Data { get; private set; }
    public bool IsAlive => currentHealth > 0;

    private void OnDestroy()
    {
        animator.OnDamageAnimationFinished -= FinishDamageAnimation;
        animator.OnAttackAnimationFinished -= FinishAttackAnimation;
    }

    public void SetData(EnemyData data)
    {
        Data = data;
        LoadModelInContainer(data.ModelPrefab);
        ResetDefaultProperties();
    }

    private void LoadModelInContainer(GameObject modelPrefab)
    {
        var model = Instantiate(modelPrefab, modelContainer);

        animator = model.AddComponent<EnemyAnimatorController>();
        animator.OnDamageAnimationFinished += FinishDamageAnimation;
        animator.OnAttackAnimationFinished += FinishAttackAnimation;
    }

    public void ResetDefaultProperties()
    {
        currentHealth = Data.MaxHealth;
        hudController.SetInitialValues(Data.MaxHealth);

        ResetRemainingTurnsToAttack();
    }

    public void ApplyDamage(int damageReceived, out int reducedHealth)
    {
        currentHealth -= damageReceived;
        reducedHealth = IsAlive ? damageReceived : damageReceived + currentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, Data.MaxHealth);

        animator.SetInteger(HealthInt, currentHealth);
        animator.SetTrigger(GetHitTrigger);

        hudController.UpdateHealth(currentHealth, HealthPercentage);
    }

    public void CheckIfShouldAttack()
    {
        remainingTurnsToAttack--;
        animator.SetInteger(RemainingTurnsToAttackInt, remainingTurnsToAttack);
    }

    private void ResetRemainingTurnsToAttack()
    {
        remainingTurnsToAttack = Data.TurnsToAttack;
        animator.SetInteger(RemainingTurnsToAttackInt, Data.TurnsToAttack);
    }

    private void FinishDamageAnimation()
    {
        OnDamageAnimationFinished?.Invoke(currentHealth > 0);
    }

    private void FinishAttackAnimation()
    {
        ResetRemainingTurnsToAttack();
        OnAttackAnimationFinished?.Invoke(Data.AttackPower);
    }
}
