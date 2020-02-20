using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private CardSetController _cardSetController;
    [SerializeField] private EnemySpawner _enemySpawner;

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

        Events.instance.Raise(new BlockCardSelectionEvent(false));
    }

    private void CardSet_OnPairFound(CardController card1, CardController card2)
    {
        //TODO: Throw at enemy and damage it
        _currentEnemy.Damage(1);

        if (!_currentEnemy.IsAlive)
        {
            Events.instance.Raise(new BlockCardSelectionEvent(true));
        }

        card1.Shrink();
        card2.Shrink(UpdateBattleStatus);
    }

    private void UpdateBattleStatus()
    {
        if (_currentEnemy.IsAlive)
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
