using UnityEngine;

public class AbstractEnemyAI : AbstractAstarAI, ISearcher 
{
    public bool canSeePlayer;
    public bool canAttackPlayer;

    public AbstractEnemyState currentState;
    public AbstractEnemyState lastState;

    public void canSeeTarget(bool canSee) 
    {
        canSeePlayer = canSee;
    }

    public void UpdatePhysics()
    {
        entity.UpdatePhysics();
    }

    public void SetMovement(Vector2 dir)
    {
        entity.SetMovement(dir);
    }

    public void Attack()
    {
        entity.Attack();
    }
}
