using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour {

    [SerializeField]
    private int damageAmount;
    private int range;
    private int attackSpeed;

    public abstract void Attack();
    public abstract void AttackAnimation();

    public int GetDamage() {
        return damageAmount;
    }
}
