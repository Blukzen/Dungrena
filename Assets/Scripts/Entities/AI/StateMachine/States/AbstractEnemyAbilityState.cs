﻿using UnityEngine;
using System.Collections;

public abstract class AbstractEnemyAbilityState : AbstractEnemyState
{
    public float coolDown = 0;
    protected Vector2 target;
    protected AbstractEntity caster;
    protected float lastCastTime = 0;

    protected override void Awake()
    {
        base.Awake();
        caster = GetComponent<AbstractEntity>();
    }

    public bool canCast()
    {
        return Time.time - lastCastTime > coolDown;
    }
}
