using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private CardSetController _cardSetController = default;
    [SerializeField] private EnemySpawner _enemySpawner = default;
    [SerializeField] private PlayerStatusController _player = default;
    [SerializeField] private Camera _camera = default;

    private EnemyController _currentEnemy;

    private void Awake()
    {
        _cardSetController.OnPairFound += CardSet_OnPairFound;
        _cardSetController.OnPairMiss += CardSet_OnPairMiss;

        Events.instance.AddListener<RestartEvent>(OnGameRestart);
    }

    private void Start()
    {
        PrepareNextEnemy();
    }

    private void OnDestroy()
    {
        _cardSetController.OnPairFound -= CardSet_OnPairFound;
        _cardSetController.OnPairMiss -= CardSet_OnPairMiss;

        Events.instance.RemoveListener<RestartEvent>(OnGameRestart);
    }

    private void PrepareNextEnemy()
    {
        if (_currentEnemy != null)
        {
            _currentEnemy.OnAttackAnimationFinished -= OnEnemyAttack;
            _currentEnemy.OnDamageAnimationFinished -= UpdateBattleStatus;
            _currentEnemy.gameObject.SetActive(false);
        }

        _currentEnemy = _enemySpawner.SpawnNewEnemy();
        _currentEnemy.OnAttackAnimationFinished += OnEnemyAttack;
        _currentEnemy.OnDamageAnimationFinished += UpdateBattleStatus;

        CreateNewGameSet();
    }

    private void CreateNewGameSet()
    {
        var enemyGrid = _currentEnemy.GridSize;
        _cardSetController.SetupNewGame(enemyGrid.x, enemyGrid.y);
    }

    private void CardSet_OnPairFound()
    {
        var isEnemyAlive = _currentEnemy.Damage(1);

        if (!isEnemyAlive)
        {
            _cardSetController.SetCardsInteractable(false);
        }
    }

    private void UpdateBattleStatus(bool isEnemyAlive)
    {
        if (isEnemyAlive)
        {
            if (_cardSetController.RemainingPairs == 0)
            {
                CreateNewGameSet();
            }
        }
        else
        {
            PrepareNextEnemy();
        }
    }

    private void CardSet_OnPairMiss()
    {
        _currentEnemy.CheckIfShouldAttack();
    }

    private void OnEnemyAttack(int atkPower)
    {
        var isPlayerAlive = _player.Damage(atkPower);
        _camera.DOShakePosition(.2f, .05f);

        if (!isPlayerAlive)
        {
            _cardSetController.SetCardsInteractable(false);

            Events.instance.Raise(new PlayerDefeatEvent());
        }
    }

    private void OnGameRestart(RestartEvent e)
    {
        _player.ResetValues();
        PrepareNextEnemy();
    }
}
