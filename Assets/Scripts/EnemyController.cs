using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform modelContainer = default;
    [SerializeField] private EnemyHUDController hudController = default;

    private const string
        GetHitTrigger = "GetHit",
        RemainingTurnsToAttackInt = "RemainingTurnsToAttack",
        HealthInt = "Health";

    private EnemyAnimatorController animator;
    private int 
        currentHealth,
        remainingTurnsToAttack,
        animationControlCount;

    private float HealthPercentage => (float)currentHealth / Data.MaxHealth;

    public Action<int> OnAtkAnimationComplete;
    public Action OnDmgAnimationComplete;

    public EnemyData Data { get; private set; }
    public bool IsAlive => currentHealth > 0;
    public Vector3 ModelPosition => modelContainer.transform.position;

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

    public void CheckIfShouldAttack()
    {
        UpdateRemainingTurnsToAttack(remainingTurnsToAttack - 1);
    }

    public void ApplyDamage(int damageReceived, out int reducedHealth)
    {
        currentHealth -= damageReceived;
        reducedHealth = IsAlive ? damageReceived : damageReceived + currentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, Data.MaxHealth);

        hudController.UpdateHealth(currentHealth, HealthPercentage);

        UpdateAnimatorState();
    }

    private void UpdateAnimatorState()
    {
        UpdateRemainingTurnsToAttack(remainingTurnsToAttack + 1);
        animator.SetInteger(HealthInt, currentHealth);
        animator.SetTrigger(GetHitTrigger);

        //This is used to avoid sending the OnDmgAnimationComplete event
        //back to the BattleManager during another hit animation
        animationControlCount++;
    }

    private void UpdateRemainingTurnsToAttack(int turnsToAttack)
    {
        remainingTurnsToAttack = turnsToAttack;
        remainingTurnsToAttack = Mathf.Clamp(remainingTurnsToAttack, 0, Data.TurnsToAttack);
        animator.SetInteger(RemainingTurnsToAttackInt, remainingTurnsToAttack);
    }

    private void Animator_OnDmgAnimationComplete()
    {
        //This checks avoid the BattleManager receiving the OnDmgAnimationComplete
        //while the death animation is playing
        if (--animationControlCount == 0)
        {
            OnDmgAnimationComplete?.Invoke();
        }
    }

    private void Animator_OnAtkAnimationComplete()
    {
        UpdateRemainingTurnsToAttack(Data.TurnsToAttack);
        OnAtkAnimationComplete?.Invoke(Data.AttackPower);
    }
}
