using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractEntity
{

    [SerializeField]
    private AbstractWeapon currentWeapon;
    public int maxMana;
    private int mana;

    private void Start() { mana = maxMana; }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
            if (currentWeapon != null)
                currentWeapon.Attack();
        else if (Input.GetMouseButtonDown(1))
            if (currentWeapon != null)
                currentWeapon.SecondaryAttack(this);
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        Vector2 targetVeloctiy = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(targetVeloctiy);

        GetComponent<Animator>().SetFloat("Velocity", targetVeloctiy.magnitude);

    }

    public bool UseMana(int amount) 
    {
        if (mana < amount)
            return false;

        mana -= amount;
        return true;
    }
}
