using UnityEngine;
using System.Collections;

public class SummonerAttackState : AbstractEnemyAbilityState
{
    public override string Name { get { return "Summoning Attack State"; } }

    public float numEnemiesToSpawn;
    public AbstractEnemyAI enemyPrefab;
    public SummoningEffectController summoningController;

    [HideInInspector]
    public int spawnCount;
    [HideInInspector]
    public bool enemySpawned;
    [HideInInspector]
    public Vector2 summoningPosition;

    public override void init()
    {
        base.init();
        Executing = true;
        spawnCount = 0;
        enemySpawned = true;
        entity.lookingAt = true;
    }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        return canCast() && enemyAI.canSeePlayer;
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        entity.StopMoving();

        if (enemySpawned && spawnCount <= numEnemiesToSpawn)
        {
            var summon = Instantiate(summoningController, enemyAI.GetCurrentRoom().GetRandomPointInRoom(2, 2), Quaternion.identity);
            entity.lookPos = summon.transform.position;

            summon.beginSummon(this, enemyPrefab);
            enemySpawned = false;
        }
    }

    public void EnemySpawned()
    {
        enemySpawned = true;
        spawnCount++;

        if (spawnCount == numEnemiesToSpawn)
        {
            Executing = false;

            entity.lookingAt = false;

            if (entity.currentWeapon != null)
                entity.currentWeapon.transform.parent.rotation = new Quaternion(0, 0, 0, 0);

            lastCastTime = Time.time;
        }
    }
}
