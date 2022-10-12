using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackInfo : AttackInfo
{
    private float _duration;
    
    public MeleeAttackInfo(float damage, float duration, Vector3 aoe, float reach, Vector3 offset) : base(damage, aoe, reach, offset)
    {
        this._duration = duration;
    }

    public float Duration { get => _duration; }

}