using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractEntity
{

    [SerializeField]
    private AbstractWeapon currentWeapon;

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
            currentWeapon.Attack();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector2 targetVeloctiy = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(targetVeloctiy);
		
	}
}
