using UnityEngine;
using System.Collections;

public class DashAbility : AbstractAbility 
{

    public float DashPower = 25;
    private Vector2 direction;

    public override void cast(AbstractEntity caster) 
    {
        if (direction.magnitude == 0)
        {
            Debug.Log("[" + caster.name + "] " + "DashAbility direction was not set");
        }

        caster.AddVelocity(direction, DashPower);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
}
