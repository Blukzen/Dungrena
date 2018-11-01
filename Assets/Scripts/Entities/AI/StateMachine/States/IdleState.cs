using UnityEngine;

public class IdleState : AbstractEnemyState
{
    public override string Name { get { return "Idle"; } }
    public override bool Executing { get { return false; } }

    public float timeIdle = 0;

    public override void init()
    {
        base.init();
        timeIdle = 0;
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        timeIdle += Time.deltaTime;
        entity.StopMoving();
    }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        // Dont idle if we can see player
        return !enemyAI.canSeePlayer;
    }
}
