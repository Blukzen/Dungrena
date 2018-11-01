using UnityEngine;

public class ChaseState : AbstractEnemyState
{
    public override string Name { get { return "Chase"; } }

    public override void init()
    {
        base.init();
        Executing = true;
    }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        return enemyAI.canSeePlayer && !enemyAI.CanAttackTarget();
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        // If can see player update path to player and move towards.
        if (enemyAI.canSeePlayer && !enemyAI.CanAttackTarget())
        {
            enemyAI.targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            enemyAI.AstarMoveToTarget();
        }

        // If reached end of path and cannot see player stop executing ChaseState.
        else
        {
            enemyAI.targetPosition = transform.position;
            Executing = false;
        }
    }
}
