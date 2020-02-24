using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyDataCollection _enemyCollection = default;
    [SerializeField] private Transform _spawnLocation = default;

    private Dictionary<int, EnemyController> _instantiatedEnemies = new Dictionary<int, EnemyController>();

    //TODO: In the future if we have a list of enemies pre defined
    //we can instantiate all of them ahead of time.
    public EnemyController SpawnNewEnemy()
    {
        var enemyData = SelectEnemyFromCollection();
        return GetEnemyController(enemyData);
    }

    private EnemyData SelectEnemyFromCollection()
    {
        var data = _enemyCollection.GetCollection();
        if (data.Length == 0)
        {
            Debug.LogError("EnemyCollection is Empty");
            return null;
        }

        var pickId = UnityEngine.Random.Range(0, data.Length);
        return data[pickId];
    }

    private EnemyController GetEnemyController(EnemyData enemyData)
    {
        EnemyController enemyController;

        if (_instantiatedEnemies.ContainsKey(enemyData.GetId()))
        {
            enemyController = _instantiatedEnemies[enemyData.GetId()];
            enemyController.SetData(enemyData);
            enemyController.gameObject.SetActive(true);
        }
        else
        {
            enemyController = Instantiate(enemyData.Prefab, _spawnLocation.position, _spawnLocation.rotation);
            enemyController.SetData(enemyData);

            _instantiatedEnemies.Add(enemyData.GetId(), enemyController);
        }

        return enemyController;
    }
}
