using UnityEngine;

public abstract class AbstractEnemyAI : AbstractAstarAI, ISearcher 
{
    public bool canSeePlayer;
    public bool attackBlocked;

    public AbstractEnemyState currentState;
    public AbstractEnemyState lastState;


    public void canAttackTarget(bool canAttack)
    {
        attackBlocked = !canAttack;
    }

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

    public void SecondaryAttack()
    {
        entity.SecondaryAttack();
    }

    public abstract bool CanAttackTarget();

    public AbstractDungeonRoom GetCurrentRoom()
    {
        AbstractDungeonRoom closestRoom = null;
        foreach (var room in GameManager.dungeonGenerator.DungeonRooms)
        {
            if (closestRoom == null)
            {
                closestRoom = room;
                continue;
            }

            if (Vector2.Distance(closestRoom.transform.position, transform.position) > Vector2.Distance(room.transform.position, transform.position))
                closestRoom = room;
        }

        return closestRoom;
    }
}
