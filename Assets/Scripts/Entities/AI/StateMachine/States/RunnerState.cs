using UnityEngine;
using System.Collections;

public class RunnerState : AbstractEnemyState
{
    public override string Name { get { return "Runner State"; } }

    private AbstractDungeonRoom currentRoom;

    public override void init()
    {
        base.init();
        currentRoom = GetCurrentRoom();
        Executing = true;
    }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        return enemyAI.canSeePlayer && !enemyAI.CanAttackTarget();
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        if (Executing && !conditionsMet(enemyAI))
        {
            Executing = false;
        }

        enemyAI.AstarMoveToTarget();

        if (enemyAI.reachedEndOfPath)
            enemyAI.targetPosition = currentRoom.GetRandomPointInRoom(0, 0);
    }

    protected AbstractDungeonRoom GetCurrentRoom()
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
