using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour, IAbility {
    public float coolDown = 0;
    public Vector2 target;
    protected AbstractEntity caster;
    protected float lastCastTime = 0;

    private void Awake()
    {
        caster = GetComponent<AbstractEntity>();
    }

    public abstract void cast();

    public bool canCast()
    {
        return Time.time - lastCastTime > coolDown;
    }
}
