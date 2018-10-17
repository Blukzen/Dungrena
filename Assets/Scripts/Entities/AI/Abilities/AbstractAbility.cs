using UnityEngine;

public abstract class AbstractAbility : IAbility {
    public abstract void cast(AbstractEntity caster);
}
