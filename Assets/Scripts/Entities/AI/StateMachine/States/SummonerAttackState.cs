using UnityEngine;
using System.Collections;

public class SummonerAttackState : AbstractEnemyAbilityState
{
    public override string Name { get { return "Summoning Attack State"; } }

    public float numEnemiesToSpawn;
    public int spawnCount;
    public bool enemySpawned;
    public AbstractEnemyAI enemyPrefab;
    public SummoningEffectController summoningController;

    public override void init()
    {
        base.init();
        Executing = true;
        spawnCount = 0;
        enemySpawned = true;
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
            // TODO: Point staff to location
            var summon = Instantiate(summoningController, enemyAI.GetCurrentRoom().GetRandomPointInRoom(2, 2), Quaternion.identity);
            summon.beginSummon(this, enemyPrefab);
            enemySpawned = false;
        }

        if (spawnCount == numEnemiesToSpawn)
        {
            Executing = false;
            lastCastTime = Time.time;
        }
    }

    public void EnemySpawned()
    {
        enemySpawned = true;
        spawnCount++;
    }
}
