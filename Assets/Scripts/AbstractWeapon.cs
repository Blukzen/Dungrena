using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour 
{
    public int damageAmount;
    public float attackSpeed;
    public int secondaryAttackManaCost;
    [HideInInspector]
    public float lastAttackTime;

    public abstract void Attack();

    public abstract void SecondaryAttack(Player player);

    public bool CanAttack() {
        Debug.Log(lastAttackTime + " : " + attackSpeed + " <= " + Time.time);
        return (lastAttackTime + attackSpeed <= Time.time); }

    public int GetDamage() { return damageAmount; }
}
