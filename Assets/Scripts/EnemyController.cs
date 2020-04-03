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

    public Action<int> OnAtkAnimationComplete;
    public Action<bool> OnDmgAnimationComplete;

    public EnemyData Data { get; private set; }
    public bool IsAlive => currentHealth > 0;

    private void OnDestroy()
    {
        animator.OnDmgAnimationComplete -= Animator_OnDmgAnimationComplete;
        animator.OnAtkAnimationComplete -= Animator_OnAtkAnimationComplete;
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
        animator.OnDmgAnimationComplete += Animator_OnDmgAnimationComplete;
        animator.OnAtkAnimationComplete += Animator_OnAtkAnimationComplete;
    }

    public void ResetDefaultProperties()
    {
        currentHealth = Data.MaxHealth;
        hudController.SetInitialValues(Data.MaxHealth);

        UpdateRemainingTurnsToAttack(Data.TurnsToAttack);
    }

    public void ApplyDamage(int damageReceived, out int reducedHealth)
    {
        currentHealth -= damageReceived;
        reducedHealth = IsAlive ? damageReceived : damageReceived + currentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, Data.MaxHealth);

        animator.SetInteger(HealthInt, currentHealth);
        animator.SetTrigger(GetHitTrigger);

        hudController.UpdateHealth(currentHealth, HealthPercentage);

        UpdateRemainingTurnsToAttack(remainingTurnsToAttack + 1);
    }

    public void CheckIfShouldAttack()
    {
        UpdateRemainingTurnsToAttack(remainingTurnsToAttack - 1);
    }

    private void UpdateRemainingTurnsToAttack(int turnsToAttack)
    {
        remainingTurnsToAttack = turnsToAttack;
        remainingTurnsToAttack = Mathf.Clamp(remainingTurnsToAttack, 0, Data.TurnsToAttack);
        animator.SetInteger(RemainingTurnsToAttackInt, remainingTurnsToAttack);
    }

    private void Animator_OnDmgAnimationComplete()
    {
        OnDmgAnimationComplete?.Invoke(currentHealth > 0);
    }

    private void Animator_OnAtkAnimationComplete()
    {
        UpdateRemainingTurnsToAttack(Data.TurnsToAttack);
        OnAtkAnimationComplete?.Invoke(Data.AttackPower);
    }
}
