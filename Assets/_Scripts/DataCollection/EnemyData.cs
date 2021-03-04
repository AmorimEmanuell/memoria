using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject, ICollectionItem
{
    [SerializeField] private int id = default;
    [SerializeField] private int health = default;
    [SerializeField] private int turnsToAttack = default;
    [SerializeField] private int attackPower = default;
    [SerializeField] private Vector2Int gridSize = default;
    [SerializeField] private GameObject model = default;
    [SerializeField] private int scorePerHealthPoint = default;

    public int MaxHealth => health;
    public int TurnsToAttack => turnsToAttack;
    public int AttackPower => attackPower;
    public Vector2Int GridSize => gridSize;
    public GameObject ModelPrefab => model;
    public int PointsPerHealth => scorePerHealthPoint;

    public int GetId()
    {
        return id;
    }
}
