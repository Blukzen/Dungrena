using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour 
{
    public int attackDamage;
    public float attackSpeed;
    public int secondaryAttackManaCost;

    [HideInInspector]
    public AbstractEntity Owner;

    private float lastAttackTime;

    public abstract void Attack();

    public abstract void SecondaryAttack();

    public bool CanAttack() 
    {
        var canAttack = lastAttackTime + attackSpeed <= Time.time;

        if (canAttack) lastAttackTime = Time.time;
        return canAttack;
    }

    public int GetDamage() { return attackDamage; }
}
