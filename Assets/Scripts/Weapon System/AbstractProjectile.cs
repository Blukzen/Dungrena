using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public abstract class AbstractProjectile : MonoBehaviour 
{
    protected AbstractEntity entityOwner;
    public AbstractEntity Owner 
    {
        get { return entityOwner; }
        set { entityOwner = value; }
    }

    protected float projectileDamage;
    public float Damage {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    protected float knockback;
    public float Knockback
    {
        get { return knockback; }
        set { knockback = value; }
    }

    protected float projectileSpeed;
    public float Speed {
        set { projectileSpeed = value; }
    }

    protected bool enemyProjectile;
    public bool EnemyProjectile
    {
        set { enemyProjectile = value; }
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() 
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;
    }
}
