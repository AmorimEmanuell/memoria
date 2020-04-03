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
        cardSetController.OnPairFound += Player_OnActionSuccess;
        cardSetController.OnPairMiss += Player_OnActionFailed;

        Events.instance.AddListener<RestartEvent>(OnGameRestart);
    }

    private void Start()
    {
        PrepareNextEnemy();
    }

    private void OnDestroy()
    {
        cardSetController.OnPairFound -= Player_OnActionSuccess;
        cardSetController.OnPairMiss -= Player_OnActionFailed;

        Events.instance.RemoveListener<RestartEvent>(OnGameRestart);
    }

    private void PrepareNextEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnAttackAnimationFinished -= Enemy_OnAttackAnimationFinished;
            currentEnemy.OnDamageAnimationFinished -= Enemy_OnDamageAnimationFinished;
            currentEnemy.gameObject.SetActive(false);
        }

        currentEnemy = enemySpawner.SpawnNewEnemy();
        currentEnemy.OnAttackAnimationFinished += Enemy_OnAttackAnimationFinished;
        currentEnemy.OnDamageAnimationFinished += Enemy_OnDamageAnimationFinished;

        CreateNewGameSet();
    }

    private void CreateNewGameSet()
    {
        var enemyGrid = currentEnemy.Data.GridSize;
        cardSetController.SetupNewGame(enemyGrid.x, enemyGrid.y);
    }

    private void Player_OnActionSuccess()
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

    private void Player_OnActionFailed()
    {
        currentEnemy.CheckIfShouldAttack();
    }

    private void Enemy_OnDamageAnimationFinished(bool isEnemyAlive)
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
            PrepareNextEnemy();
        }
    }

    private void Enemy_OnAttackAnimationFinished(int atkPower)
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
