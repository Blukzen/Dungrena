using UnityEngine;
using System.Collections;

public class DashAbility : AbstractAbility 
{

    public override void cast(AbstractEntity caster) 
    {
        caster.AddVelocity(Vector2.left, 50);
    }
}
