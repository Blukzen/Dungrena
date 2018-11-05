using UnityEngine;
using System.Collections;

public class SummoningEffectController : MonoBehaviour
{
    private AbstractEnemyAI enemySummoned;
    private AbstractEnemyAI enemyToSummon;
    private SummonerAttackState summoner;
    private Animator animator;

    public void beginSummon(SummonerAttackState _summoner, AbstractEnemyAI _enemyToSummon)
    {
        enemyToSummon = _enemyToSummon;
        summoner = _summoner;
        animator = GetComponent<Animator>();
        animator.SetTrigger("BeginSummon");
    }

    public void finishSummon()
    {
        summoner.EnemySpawned();
        enemySummoned.transform.parent = GameManager.dungeonGenerator.Enemies.transform;
        enemySummoned.enabled = true;
        Destroy(this.gameObject);
    }

    public void summonEnemy()
    {
        enemySummoned = Instantiate(enemyToSummon, GameManager.dungeonGenerator.Enemies.transform);
        enemySummoned.transform.position = this.transform.position;
        enemySummoned.enabled = false;
    }
}
