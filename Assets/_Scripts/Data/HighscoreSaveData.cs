using System;
using UnityEngine;

[Serializable]
public class HighscoreSaveData : ISaveData
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
        var fileExists = FileUtils.TryLoad<HighscoreSaveData>(Filename, out var highscoreSaveData);
        if (!fileExists)
        {
            highscoreSaveData = GetInitialValues();
        }

        return highscoreSaveData;
    }

    public static void UpdateNewHighscore(uint newHighscore)
    {
        var highscoreSaveData = Load();
        highscoreSaveData.SetHighscore(newHighscore);
        FileUtils.Save(Filename, highscoreSaveData);
    }

    private static HighscoreSaveData GetInitialValues()
    {
        return new HighscoreSaveData(0);
    }
}
