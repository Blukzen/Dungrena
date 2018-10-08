using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityLookAtMouse : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
    {
        transform.rotation = DirectionToMouse();
	}

    private Quaternion DirectionToMouse() 
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
