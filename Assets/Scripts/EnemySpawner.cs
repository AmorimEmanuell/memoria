using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyDataCollection _enemyCollection = default;
    [SerializeField] private Transform _spawnLocation = default;

    //TODO: In the future if we have a list of enemies pre defined
    //we can instantiate all of them ahead of time.
    public EnemyController SpawnNewEnemy()
    {
        var enemyData = SelectEnemyFromCollection();
        var enemyController = Instantiate(enemyData.Prefab, _spawnLocation.position, _spawnLocation.rotation);
        enemyController.SetData(enemyData);
        return enemyController;
    }

    private EnemyData SelectEnemyFromCollection()
    {
        var data = _enemyCollection.GetCollection();
        if (data.Length == 0)
        {
            Debug.LogError("EnemyCollection is Empty");
            return null;
        }

        var pickId = UnityEngine.Random.Range(0, data.Length - 1);
        return data[pickId];
    }
}
