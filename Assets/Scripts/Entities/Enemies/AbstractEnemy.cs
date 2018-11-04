﻿using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : AbstractEntity
{
    private AbstractAbility attack;
    public Player target;

    public float AbilityCooldown;
    private float lastAbilityAttack;

    [HideInInspector]
    public GameObject weaponHolder;
    [HideInInspector]
    public AbstractWeapon currentWeapon;
    [HideInInspector]
    public bool lookingAt = false;
    [HideInInspector]
    public Vector2 lookPos;

    public override void Awake()
    {
        base.Awake();

        attack = GetComponent<AbstractAbility>();
        target = GameManager.player;

        if (transform.Find("Weapon"))
            weaponHolder = transform.Find("Weapon").gameObject;
    }

    public override bool CanSecondaryAttack()
    {
        return Time.time - lastAbilityAttack >= AbilityCooldown && currentWeapon.CanAttack();
    }

    public void Attack()
    {
        if (currentWeapon == null)
        {
            Debug.Log("[" + name + "] " + "Has no weapon to attack with!");
            return;
        }

        currentWeapon.Attack();
    }

    public void SecondaryAttack()
    {
        if (currentWeapon == null)
        {
            Debug.Log("[" + name + "]" + "Has no weapon to attack with!");
            return;
        }

        currentWeapon.SecondaryAttack();
        lastAbilityAttack = Time.time;
    }

    protected override void UpdateSprite()
    {
        base.UpdateSprite();

        if (currentWeapon != null)
            currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;

        var newScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (lookingAt && lookPos != null)
        {
            if (transform.position.x < lookPos.x)
                newScale.x = 1;
            else if (transform.position.x > lookPos.x)
                newScale.x = -1;

        } else
        {
            if (rb2d.velocity.x < -0.5)
                newScale.x = -1;
            else if (rb2d.velocity.x > 0.5)
                newScale.x = 1;
        }

        transform.localScale = newScale;
    }

    // TODO: Death effect
    public override void Killed()
    {
        base.Killed();
        Destroy(gameObject);
    }
}
