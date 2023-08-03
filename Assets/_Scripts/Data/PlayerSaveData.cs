using System;
using UnityEngine;

[Serializable]
public class PlayerSaveData : ISaveData
{
    [SerializeField] private int _maxHealth, _attackPower, _maxPotions, _potionStrength;

    private const string Filename = "Player.savedata";

    public int MaxHealth => _maxHealth;
    public int AttackPower => _attackPower;
    public int MaxPotions => _maxPotions;
    public int PotionStrength => _potionStrength;

    private PlayerSaveData(int maxHealth, int attackPower, int maxPotions, int potionStrength)
    {
        _maxHealth = maxHealth;
        _attackPower = attackPower;
        _maxPotions = maxPotions;
        _potionStrength = potionStrength;
    }

    public static PlayerSaveData Load()
    {
        var fileExists = FileUtils.TryLoad<PlayerSaveData>(Filename, out var playerSaveData);
        if (!fileExists)
        {
            playerSaveData = GetInitialValues();
        }

        return playerSaveData;
    }

    private static PlayerSaveData GetInitialValues()
    {
        return new PlayerSaveData(3, 1, 3, 1);
    }
}
