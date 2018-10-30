using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractEntity
{
    public int maxMana;
    private int mana;

    [Header("Extra")]
    [SerializeField]
    private AbstractWeapon currentWeapon;
    private AbstractAbility ability;

    private void Start() {
        mana = maxMana;
        ability = GetComponent<AbstractAbility>();

        // Check if we started with a weapon in hand
        var weapon = GetComponentInChildren<AbstractWeapon>();
        if (weapon != null)
            weapon.pickup(this);
    }

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
            pickupItem();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        SetMovement(direction);
        UpdatePhysics();    
        

        GetComponent<Animator>().SetFloat("Movement", direction.magnitude);
    }

    public bool UseMana(int amount) 
    {
        if (mana < amount)
            return false;

        mana -= amount;
        return true;
    }

    public void EquipWeapon(AbstractWeapon weapon)
    {
        if (currentWeapon != null)
            currentWeapon.Drop(weapon.transform.position);

        weapon.transform.parent = transform.Find("Weapon");
        currentWeapon = weapon;
    }
}
