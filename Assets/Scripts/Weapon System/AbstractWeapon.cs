using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : AbstractItem
{
    public int attackDamage;
    public float attackSpeed;
    public float secondaryAttackDamage;
    public int secondaryAttackManaCost;

    protected float lastAttackTime = 0;

    public abstract void Attack();
    public abstract void SecondaryAttack();

    protected virtual void Start()
    {
        ItemType = ItemType.Weapon;
    }

    public bool CanAttack() 
    {
        return Time.time - lastAttackTime >= attackSpeed;
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
