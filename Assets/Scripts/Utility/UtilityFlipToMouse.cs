using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFlipToMouse : MonoBehaviour 
{
	// Update is called once per frame
	void Update () {

        var flip = MousePositionDifference();
        var newScale = new Vector3(0, transform.localScale.y, transform.localScale.z);

        if (flip > 0) {
            newScale.x = 1;
        } else if (flip < 0) {
            newScale.x = -1;
        }

        transform.localScale = newScale;
		
	}

    private float MousePositionDifference() {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        return diff.x;
    }
}
