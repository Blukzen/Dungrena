using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : AbstractItem
{
    public float attackDamage;
    public float attackSpeed;
    public float secondaryAttackDamage;
    public int secondaryAttackManaCost;
    public float knockBackForce; // Knockback force applied to hit enemy.
    public WeaponType weaponType;

    [HideInInspector]
    public bool enemyWeapon;

    protected float currentDamage; // This value is the damage of the current attack/damage applied on hit.
    protected float currentManaCost;

    protected float lastAttackTime = 0;
    public bool attacking = false;

    public abstract void Attack();
    public abstract void SecondaryAttack();

    protected virtual void Start()
    {
        ItemType = ItemType.Weapon;
        transform.Rotate(0, 0, Random.Range(0, 360));
    }

    public bool CanAttack() 
    {
        return Time.time - lastAttackTime >= 1/attackSpeed && !attacking;
    }

    public bool CanSecondaryAttack()
    {
        return Owner.CanSecondaryAttack() && !attacking;
    }

    public override void MouseOver()
    {
        base.MouseOver();
        UIManager.ShowWeaponInfo(this);
    }

    public override void MouseExit()
    {
        base.MouseExit();
        UIManager.HideWeaponInfo();
    }
}

public enum WeaponType
{
    MELEE, PROJECTILE
}
