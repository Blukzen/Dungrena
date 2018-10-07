using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public abstract class AbstractProjectile : MonoBehaviour 
{
    private AbstractEntity entityOwner;
    public AbstractEntity Owner 
    {
        get { return entityOwner; }
        set { entityOwner = value; }
    }

    private int projectileDamage;
    public int Damage {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    private int projectileSpeed;
    public int Speed {
        set { projectileSpeed = value; }
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() 
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;
    }
}
