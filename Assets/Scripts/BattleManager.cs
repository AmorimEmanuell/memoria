using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private CardSetController cardSetController = default;
    [SerializeField] private EnemySpawner enemySpawner = default;
    [SerializeField] private PlayerStatus player = default;

    private EnemyController currentEnemy;

    private void Awake()
    {
        cardSetController.OnPairFound += CardSet_OnPairFound;
        cardSetController.OnPairMiss += CardSet_OnPairMiss;

        Events.instance.AddListener<RestartEvent>(OnGameRestart);
    }

    private void Start()
    {
        PrepareNextEnemy();
    }

    private void OnDestroy()
    {
        cardSetController.OnPairFound -= CardSet_OnPairFound;
        cardSetController.OnPairMiss -= CardSet_OnPairMiss;

        Events.instance.RemoveListener<RestartEvent>(OnGameRestart);
    }

    private void PrepareNextEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnAttackAnimationFinished -= OnEnemyAttack;
            currentEnemy.OnDamageAnimationFinished -= UpdateBattleStatus;
            currentEnemy.gameObject.SetActive(false);
        }

        currentEnemy = enemySpawner.SpawnNewEnemy();
        currentEnemy.OnAttackAnimationFinished += OnEnemyAttack;
        currentEnemy.OnDamageAnimationFinished += UpdateBattleStatus;

        CreateNewGameSet();
    }

    private void CreateNewGameSet()
    {
        var enemyGrid = currentEnemy.Data.GridSize;
        cardSetController.SetupNewGame(enemyGrid.x, enemyGrid.y);
    }

    private void CardSet_OnPairFound()
    {
        var playerAttackPower = player.Data.AttackPower;
        currentEnemy.ApplyDamage(playerAttackPower, out int reducedHealth);

        if (currentEnemy.IsAlive)
        {
            var score = currentEnemy.Data.PointsPerHealth * reducedHealth;
            player.IncreaseScore(score);
        }
        else
        {
            cardSetController.SetCardsInteractable(false);
        }
    }

    private void UpdateBattleStatus(bool isEnemyAlive)
    {
        if (isEnemyAlive)
        {
            if (cardSetController.RemainingPairs == 0)
            {
                CreateNewGameSet();
            }
        }
        else
        {
            player.IncreaseScore(currentEnemy.Data.RewardPoints);
            PrepareNextEnemy();
        }
    }

    private void CardSet_OnPairMiss()
    {
        currentEnemy.CheckIfShouldAttack();
    }

    private void OnEnemyAttack(int atkPower)
    {
        var isPlayerAlive = player.ApplyDamage(atkPower);

        if (!isPlayerAlive)
        {
            cardSetController.SetCardsInteractable(false);
            Events.instance.Raise(new PlayerDefeatEvent());
        }
    }

    private void OnGameRestart(RestartEvent e)
    {
        player.ResetDefaultValues();
        PrepareNextEnemy();
    }
}
