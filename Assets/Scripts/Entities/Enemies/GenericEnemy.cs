using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : AbstractEntity {

    private AbstractAbility attack;
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePhysics();
	}

    void Attack()
    {
        attack.cast(this);
    }
}
