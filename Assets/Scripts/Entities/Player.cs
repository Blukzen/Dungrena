using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractEntity
{
    public float maxMana;
    public float mana;
    public float manaRegen;
    private float lastManaRegen;

    public AbstractWeapon currentWeapon;

    public float damageShakeAmount = 0.15f;

    private AbstractItem selectedItem;

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0)) {
            if (currentWeapon != null) currentWeapon.Attack();
        } else if (Input.GetMouseButtonDown(1)) {
            if (currentWeapon != null)
                currentWeapon.SecondaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }

        MouseOverItem();
        ManaRegen();

    }

    // Raycast version of OnMouseOver
    private void MouseOverItem()
    {
        Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << LayerMask.NameToLayer("Items"));

        if (collider == null)
        {
            if (selectedItem != null)
            {
                selectedItem.MouseExit();
                selectedItem = null;
            }

            return;
        }

        selectedItem = collider.GetComponent<AbstractItem>();
        selectedItem.MouseOver();
    }

    // Update is called once per frame
    void FixedUpdate () 
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        SetMovement(direction);
        UpdatePhysics();    
        

        GetComponent<Animator>().SetFloat("Movement", direction.magnitude);
    }

    public override void ApplyAttack(float damage, float knockback, AbstractEntity attacker)
    {
        base.ApplyAttack(damage, knockback, attacker);
        UIManager.UpdateHealth(health, maxHealth);
    }

    public override void DamageEffectPlay()
    {
        base.DamageEffectPlay();
        var CameraShake = Camera.main.GetComponent<CameraShake>();
        if (CameraShake != null)
            StartCoroutine(CameraShake.Shake(damageShakeAmount, 0.2f));
    }

    protected override void UpdateSprite()
    {
        base.UpdateSprite();
        if (currentWeapon != null)
            currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder -1;
    }

    public bool UseMana(float amount) 
    {
        if (mana < amount)
            Debug.LogWarning("[Player] " + "Using more mana than we have!");

        mana -= amount;

        UIManager.UpdateMana(mana, maxMana);

        return true;
    }
    
    protected void ManaRegen()
    {
        if (mana == maxMana)
            return;

        if (Time.time - lastManaRegen < 1 / manaRegen)
            return;

        mana += 1;
        lastManaRegen = Time.time;
        UIManager.UpdateMana(mana, maxMana);
    }

    public override void EquipItem(AbstractWeapon weapon)
    {
        if (currentWeapon != null)
            currentWeapon.Drop();

        var weaponHolder = transform.Find("Weapon");
        weaponHolder.transform.rotation = new Quaternion(0, 0, 0, 0);

        currentWeapon = weapon;
        currentWeapon.Pickup(this);
        currentWeapon.transform.parent = weaponHolder;
        currentWeapon.transform.localScale = new Vector3(1, 1, 1);

        if (currentWeapon.weaponType == WeaponType.PROJECTILE)
            gameObject.GetComponentInChildren<UtilityLookAtMouse>().enabled = true;
        else
            gameObject.GetComponentInChildren<UtilityLookAtMouse>().enabled = false;
    }

    public override void OnAttackBegin(float manaCost)
    {
        base.OnAttackBegin(manaCost);
        UseMana(manaCost);
    }

    public override bool CanSecondaryAttack()
    {
        return mana >= currentWeapon.secondaryAttackManaCost;
    }

    public AbstractDungeonRoom GetCurrentRoom()
    {
        AbstractDungeonRoom closestRoom = null;
        foreach (var room in GameManager.dungeonGenerator.DungeonRooms)
        {
            if (closestRoom == null)
            {
                closestRoom = room;
                continue;
            }

            if (Vector2.Distance(closestRoom.transform.position, transform.position) > Vector2.Distance(room.transform.position, transform.position))
                closestRoom = room;
        }

        return closestRoom;
    }
}
