using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWeapon : AbstractWeapon 
{
    public AbstractProjectile projectile;
    public float projectileSpeed;
    public Transform firePosition;
    public string mainAttackAnimation;
    public string abilityAttackAnimation;
    

    private Vector2 Target;

    protected virtual void AttackBegin()
    {
        attacking = true;
        Owner.OnAttackBegin(currentManaCost);
    }

    protected virtual void AttackEnd()
    {
        attacking = false;
        lastAttackTime = Time.time;
        Owner.OnAttackEnd();
    }

    protected void Shoot() 
    {
        var proj = Instantiate(projectile, firePosition.transform.position, transform.parent.rotation);
        proj.Owner = transform.parent.parent.GetComponent<AbstractEntity>();
        proj.Speed = projectileSpeed;
        proj.Damage = currentDamage;
        proj.Knockback = knockBackForce;

        if (enemyWeapon)
            proj.EnemyProjectile = true;
    }

    protected void ShootStaffDir()
    {
        var proj = Instantiate(projectile, firePosition.transform.position, transform.rotation);
        proj.Owner = transform.parent.parent.GetComponent<AbstractEntity>();
        proj.Speed = projectileSpeed;
        proj.Damage = currentDamage;
        proj.Knockback = knockBackForce;

        if (enemyWeapon)
            proj.EnemyProjectile = true;
    }

    protected Quaternion DirectionToPoint(Vector3 point) 
    {
        Vector3 diff = point - firePosition.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
