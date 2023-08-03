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
