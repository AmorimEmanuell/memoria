using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private CardSetController cardSetController = default;
    [SerializeField] private EnemySpawner enemySpawner = default;
    [SerializeField] private PlayerController player = default;
    [SerializeField] private PotionSpawner potionSpawner = default;

#if D
    private const float PotionDroppingChance = 1f;
#else
    private const float PotionDroppingChance = 0.1f;
#endif

    private EnemyController currentEnemy;

    private void Awake()
    {
        cardSetController.OnPairFound += Player_OnActionSucceded;
        cardSetController.OnPairMiss += Player_OnActionFailed;

        Events.instance.AddListener<RestartEvent>(OnGameRestart);
    }

    private void Start()
    {
        AdvanceToNextEnemy();
    }

    private void OnDestroy()
    {
        cardSetController.OnPairFound -= Player_OnActionSucceded;
        cardSetController.OnPairMiss -= Player_OnActionFailed;

        Events.instance.RemoveListener<RestartEvent>(OnGameRestart);
    }

    private void AdvanceToNextEnemy()
    {
        PrepareNextEnemy();
        PrepareNextGameRound();
    }

    private void PrepareNextEnemy()
    {
        DeactivateCurrentEnemy();

        currentEnemy = enemySpawner.SpawnNewEnemy();
        currentEnemy.OnAtkAnimationComplete += Enemy_OnAtkAnimationComplete;
        currentEnemy.OnDmgAnimationComplete += Enemy_OnDmgAnimationComplete;
    }

    private void DeactivateCurrentEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnAtkAnimationComplete -= Enemy_OnAtkAnimationComplete;
            currentEnemy.OnDmgAnimationComplete -= Enemy_OnDmgAnimationComplete;
            currentEnemy.gameObject.SetActive(false);
        }
    }

    private void PrepareNextGameRound()
    {
        var enemyGrid = currentEnemy.Data.GridSize;
        cardSetController.SetupNewGame(enemyGrid.x, enemyGrid.y);
    }

    private void Player_OnActionSucceded()
    {
        currentEnemy.ApplyDamage(player.Data.AttackPower, out int reducedHealth);
        player.AddScore(currentEnemy.Data.PointsPerHealth * reducedHealth);

        if (!currentEnemy.IsAlive)
        {
            cardSetController.SetCardsInteractable(false);
        }
    }

    private void Player_OnActionFailed()
    {
        currentEnemy.CheckIfShouldAttack();
    }

    private void Enemy_OnDmgAnimationComplete()
    {
        if (currentEnemy.IsAlive)
        {
            if (cardSetController.RemainingPairs == 0)
            {
                PrepareNextGameRound();
            }
        }
        else
        {
            CalculatePotionDroppingChance();
            AdvanceToNextEnemy();
        }
    }

    private void CalculatePotionDroppingChance()
    {
        if (Random.value <= PotionDroppingChance)
        {
            potionSpawner.Spawn(currentEnemy.ModelPosition);
        }
    }

    private void Enemy_OnAtkAnimationComplete(int atkPower)
    {
        player.ApplyDamage(atkPower);

        if (!player.IsAlive)
        {
            PrepareGameOver();
        }
    }

    private void PrepareGameOver()
    {
        cardSetController.SetCardsInteractable(false);
        player.ActivateScore(false);

        DeactivateCurrentEnemy();
        cardSetController.ClearRemainingCards();

        Events.instance.Raise(new PlayerDefeatEvent(player.CurrentScore));
    }

    private void OnGameRestart(RestartEvent e)
    {
        player.InitializeState();
        AdvanceToNextEnemy();
    }
}
