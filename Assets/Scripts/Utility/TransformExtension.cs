using UnityEngine;
using System.Collections;

public static class TransformExtension
{
    public static void LookAtPoint(this Transform trans, Vector3 point)
    {
        Vector3 diff = point - trans.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
