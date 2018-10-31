using System.Collections;
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
    protected Animator animator;
    protected static Material outlineMaterial;
    protected static Material defaultMaterial;

    public abstract void Attack();
    public abstract void SecondaryAttack();

    private void Awake()
    {
        if (outlineMaterial == null)
            outlineMaterial = new Material(Shader.Find("Custom/Sprite Outline"));

        if (defaultMaterial == null)
            defaultMaterial = new Material(Shader.Find("Sprites/Default"));

        animator = GetComponent<Animator>();
    }

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
        Owner = entity;
        onGround = false;
    }

    public virtual void Drop(Vector2 position)
    {
        transform.parent = null;
        transform.position = position;
        onGround = true;
    }

    private void OnMouseEnter()
    {
        if (!onGround)
            return;

        GetComponent<SpriteRenderer>().material = outlineMaterial;
    }

    private void OnMouseExit()
    {
        if (!onGround)
            return; 

        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }
}
