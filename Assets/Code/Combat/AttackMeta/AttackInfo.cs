using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Generic AttackInfo class to be used to encapsulate damage data for different entities
*/
public class AttackInfo 
{
    private float _damage;
    private float _aoe;
    private float _reach;
    private Vector3 _offset;
    public AttackInfo(float damage, float aoe, float reach, Vector3 offset) 
    {
        this._damage = damage;
        this._aoe = aoe;
        this._reach = reach;
        this._offset = offset;
    }

    public float Damage { get => _damage; set => _damage = value; }
    public float Aoe { get => _aoe; }
    public Vector3 Offset { get => _offset; }
    public float Reach { get => _reach; }

}