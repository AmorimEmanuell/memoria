using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class HighscoreDataManager
{
    static HighscoreDataManager()
    {
        Events.instance.RemoveListener<PlayerDefeatEvent>(OnPlayerDefeatEvent);
        Events.instance.AddListener<PlayerDefeatEvent>(OnPlayerDefeatEvent);
    }

    private static void OnPlayerDefeatEvent(PlayerDefeatEvent e)
    {
        var currentHighscore = HighscoreSaveData.Load().Highscore;
        if (e.FinalScore > currentHighscore)
        {
            HighscoreSaveData.UpdateNewHighscore((uint)e.FinalScore);
        }
    }

    public static uint GetHighscore()
    {
        return HighscoreSaveData.Load().Highscore;
    }
}

[Serializable]
public class HighscoreSaveData
{
    [SerializeField] private uint _highscore;

    private const string Filename = "Highscore.savedata";

    public uint Highscore => _highscore;

    public HighscoreSaveData(uint highscore)
    {
        _highscore = highscore;
    }

    private void SetHighscore(uint highscore)
    {
        _highscore = highscore;
    }

    public static HighscoreSaveData Load()
    {
        var filePath = Path.Combine(Application.persistentDataPath, Filename);

        var bf = new BinaryFormatter();
        HighscoreSaveData highscoreSaveData;

        if (File.Exists(filePath))
        {
            var fs = File.OpenRead(filePath);
            highscoreSaveData = (HighscoreSaveData)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            highscoreSaveData = GetInitialValues();
        }

        return highscoreSaveData;
    }

    public static void UpdateNewHighscore(uint newHighscore)
    {
        var filePath = Path.Combine(Application.persistentDataPath, Filename);

        var bf = new BinaryFormatter();
        HighscoreSaveData highscoreSaveData;

        if (File.Exists(filePath))
        {
            var fs = File.OpenRead(filePath);
            highscoreSaveData = (HighscoreSaveData)bf.Deserialize(fs);
            highscoreSaveData.SetHighscore(newHighscore);

            bf.Serialize(fs, highscoreSaveData);
            fs.Close();
        }
        else
        {
            highscoreSaveData = GetInitialValues();
            highscoreSaveData.SetHighscore(newHighscore);

            var fs = File.Create(filePath);
            bf.Serialize(fs, highscoreSaveData);
            fs.Close();
        }
    }

    private static HighscoreSaveData GetInitialValues()
    {
        return new HighscoreSaveData(0);
    }
}
