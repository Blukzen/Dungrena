using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : AbstractItem
{
    public int attackDamage;
    public float attackSpeed;
    public float secondaryAttackDamage;
    public int secondaryAttackManaCost;

    private float lastAttackTime;

    public abstract void Attack();
    public abstract void SecondaryAttack();

    protected virtual void Start()
    {
        ItemType = ItemType.Weapon;
    }

    public bool CanAttack() 
    {
        var canAttack = lastAttackTime + attackSpeed <= Time.time;

        if (canAttack) lastAttackTime = Time.time;
        return canAttack;
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
