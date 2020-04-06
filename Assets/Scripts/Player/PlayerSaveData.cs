using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    [SerializeField] private int
        _maxHealth,
        _attackPower,
        _maxPotions,
        _potionStrength;

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
        var filePath = Path.Combine(Application.persistentDataPath, Filename);

        BinaryFormatter bf = new BinaryFormatter();
        PlayerSaveData playerData;

        if (File.Exists(filePath))
        {
            FileStream fs = File.OpenRead(filePath);
            playerData = (PlayerSaveData)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            playerData = GetInitialValues();
            FileStream fs = File.Create(filePath);
            bf.Serialize(fs, playerData);
            fs.Close();
        }

        return playerData;
    }

    private static PlayerSaveData GetInitialValues()
    {
        return new PlayerSaveData(3, 1, 3, 1);
    }
}
