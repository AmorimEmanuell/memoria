using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject, ICollectionItem
{
    [SerializeField] private int _id = default;
    [SerializeField] private int _health = default;
    [SerializeField] private int _turnsToAttack = default;
    [SerializeField] private int _attackPower = default;
    [SerializeField] private Vector2Int _gridSize = default;
    [SerializeField] private GameObject _model = default;

    public int Health => _health;
    public int TurnsToAttack => _turnsToAttack;
    public int AttackPower => _attackPower;
    public Vector2Int GridSize => _gridSize;
    public GameObject Model => _model;

    public int GetId()
    {
        return _id;
    }
}
