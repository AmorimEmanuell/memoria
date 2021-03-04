public class PlayerDefeatEvent : GameEvent
{
    private int finalScore;

    public int FinalScore => finalScore;

    public PlayerDefeatEvent(int finalScore)
    {
        this.finalScore = finalScore;
    }
}
