public class EnemyDefeatEvent : GameEvent
{
    public EnemyController EnemyController { get; private set; }

    public EnemyDefeatEvent(EnemyController enemyController)
    {
        EnemyController = enemyController;
    }
}
