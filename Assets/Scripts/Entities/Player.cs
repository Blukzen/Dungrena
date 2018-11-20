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
        if (Input.GetMouseButtonDown(0))
        {
            if (currentWeapon != null) currentWeapon.Attack();
        }
        else if (Input.GetMouseButtonDown(1))
        {
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
    void FixedUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        SetMovement(direction);
        UpdatePhysics();


        GetComponent<Animator>().SetFloat("Movement", direction.magnitude);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        gameObject.GetComponent<UtilityFlipToMouse>().enabled = canMove;
    }

    public override void ApplyAttack(float damage, float knockback, AbstractEntity attacker)
    {
        base.ApplyAttack(damage, knockback, attacker);
        UIManager.UpdateHealth(health, maxHealth);
    }

    public override void ApplyAttack(float damage)
    {
        base.ApplyAttack(damage);
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
            currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;
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

    public void Heal(int amount)
    {
        if (health + amount > maxHealth) health = maxHealth;
        else health += amount;

        UIManager.UpdateHealth(health, maxHealth);
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

    protected override IEnumerable FallingAnim()
    {
        float yShrinkRate = 0.009f;
        float xShrinkRate = 0.009f;

        // In case flipped
        xShrinkRate *= transform.localScale.x;

        while (transform.localScale.y > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - xShrinkRate, transform.localScale.y - yShrinkRate);
            transform.position = new Vector2(transform.position.x, transform.position.y - yShrinkRate);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - yShrinkRate);

            if (currentWeapon != null)
            {
                currentWeapon.GetComponent<SpriteRenderer>().color = sprite.color;
            }

            if (transform.localScale.y < 0.9)
            {
                sprite.sortingLayerName = "Default";
                sprite.sortingOrder = -500000;

                if (currentWeapon != null)
                {
                    currentWeapon.GetComponent<SpriteRenderer>().sortingLayerName = sprite.sortingLayerName;
                    currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

        FinishedFall();
    }

    public override void Killed()
    {
        GameManager.instance.GameOver();
        Destroy(gameObject);
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
