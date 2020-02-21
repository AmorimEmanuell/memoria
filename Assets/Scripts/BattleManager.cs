using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private CardSetController _cardSetController = default;
    [SerializeField] private EnemySpawner _enemySpawner = default;

    private EnemyController _currentEnemy;

    private void Awake()
    {
        _cardSetController.OnPairFound += CardSet_OnPairFound;
        _cardSetController.OnPairMiss += CardSet_OnPairMiss;
    }

    private void Start()
    {
        CreateNewMatch();
    }

    private void OnDestroy()
    {
        _cardSetController.OnPairFound -= CardSet_OnPairFound;
        _cardSetController.OnPairMiss -= CardSet_OnPairMiss;
    }

    private void CreateNewMatch()
    {
        if (_currentEnemy != null)
        {
            _currentEnemy.gameObject.SetActive(false);
        }

        _currentEnemy = _enemySpawner.SpawnNewEnemy();
        _cardSetController.CreateNewSet(_currentEnemy.Data.GridSize.x, _currentEnemy.Data.GridSize.y);
    }

    private void CardSet_OnPairFound()
    {
        var isEnemyAlive = _currentEnemy.Damage(1, UpdateBattleStatus);

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
                _cardSetController.CreateNewSet(_currentEnemy.Data.GridSize.x, _currentEnemy.Data.GridSize.y);
            }
        }
        else
        {
            CreateNewMatch();
        }
    }

    private void CardSet_OnPairMiss()
    {
        //Verify if enemy makes attack
        //throw new NotImplementedException();
    }
}
