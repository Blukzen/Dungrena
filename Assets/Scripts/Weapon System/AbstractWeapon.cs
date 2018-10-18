﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour, IPickupable
{
    public int attackDamage;
    public float attackSpeed;
    public int secondaryAttackManaCost;

    [HideInInspector]
    public AbstractEntity Owner;

    private float lastAttackTime;

    protected bool onGround = true;

    public abstract void Attack();
    public abstract void SecondaryAttack();

    public bool CanAttack() 
    {
        var canAttack = lastAttackTime + attackSpeed <= Time.time;

        if (canAttack) lastAttackTime = Time.time;
        return canAttack;
    }

    public int GetDamage() { return attackDamage; }

    public virtual void pickup(AbstractEntity entity)
    {
        ((Player)entity).EquipWeapon(this);
        onGround = false;
    }

    public virtual void Drop(Vector2 position)
    {
        transform.parent = null;
        transform.position = position;
        onGround = true;
    }
}
