using UnityEngine;
using System.Collections;

public class OrcSummonerAI : AbstractEnemyAI
{
    private IdleState idleState;
    private PatrolState patrolState;
    private RunnerState runnerState;
    private SummonerAttackState summonerState;

    private Animator animator;

    private void Start()
    {
        idleState = GetComponent<IdleState>();
        patrolState = GetComponent<PatrolState>();
        runnerState = GetComponent<RunnerState>();
        summonerState = GetComponent<SummonerAttackState>();

        animator = GetComponent<Animator>();
    
        currentState = idleState;
        entity.EquipItem(transform.Find("Weapon").GetComponentInChildren<AbstractWeapon>());
        entity.currentWeapon.enemyWeapon = true;
    }

    private void FixedUpdate()
    {
        // Dont update state if the current state is not yet finished
        if (!currentState.Executing)
            UpdateState();

        currentState.execute(this);

        entity.UpdatePhysics();

        if (currentState == idleState || currentState == summonerState)
            animator.SetTrigger("Idle");

        if (currentState == patrolState || currentState == runnerState)
            animator.SetTrigger("Walking");

        if (currentState == summonerState)
            entity.weaponHolder.transform.LookAtPoint(entity.lookPos);
    }

    private void UpdateState()
    {
        AbstractEnemyState newState = null;

        if (idleState.conditionsMet(this) && currentState.Name != idleState.Name)
        {
            newState = idleState;
        }

        if (patrolState.conditionsMet(this) && currentState.Name != patrolState.Name)
        {
            newState = patrolState;
        }

        if (runnerState.conditionsMet(this) && currentState.Name != runnerState.Name)
            newState = runnerState;

        if (summonerState.conditionsMet(this) && currentState.Name != summonerState.Name)
            newState = summonerState;


        if (newState != null && newState != currentState)
        {
            newState.init();
            lastState = currentState;
            currentState = newState;
        }
    }

    public override bool CanAttackTarget()
    {
        return summonerState.canCast() && canSeePlayer;
    }
}
