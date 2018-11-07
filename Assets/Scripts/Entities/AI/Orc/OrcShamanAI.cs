using UnityEngine;
using System.Collections;

public class OrcShamanAI : AbstractEnemyAI
{
    private IdleState idleState;
    private PatrolState patrolState;
    private ChaseState chaseState;
    private ProjectileWeaponAttackState attackState;

    private Animator animator;

    private void Start()
    {
        idleState = GetComponent<IdleState>();
        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent <ProjectileWeaponAttackState>();

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

        if (canSeePlayer)
        {
            if (GameManager.player == null)
                return;

            entity.weaponHolder.transform.LookAtPoint(GameManager.player.transform.position);
        }
        else
        {
            entity.weaponHolder.transform.rotation = new Quaternion();
        }

        currentState.execute(this);

        entity.UpdatePhysics();

        if (currentState == idleState || currentState == attackState)
            animator.SetTrigger("Idle");

        if (currentState == patrolState || currentState == chaseState)
            animator.SetTrigger("Walking");
    }

    private void UpdateState()
    {
        AbstractEnemyState newState = null;

        if (idleState.conditionsMet(this) && currentState.Name != idleState.Name)
            newState = idleState;

        if (patrolState.conditionsMet(this) && currentState.Name != patrolState.Name)
            newState = patrolState;

        if (chaseState.conditionsMet(this) && currentState.Name != chaseState.Name)
            newState = chaseState;

        if (attackState.conditionsMet(this) && currentState.Name != attackState.Name)
            newState = attackState;

        if (newState != null && newState != currentState)
        {
            newState.init();
            lastState = currentState;
            currentState = newState;
        }
    }

    public override bool CanAttackTarget()
    {
        return canSeePlayer && attackState.TargetInRange() && !attackBlocked;
    }
}
