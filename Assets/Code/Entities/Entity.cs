using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityController _controller;

    [SerializeField] float _speed;

    private AttackInfo _attackType;
    protected void Awake() {
        this._health = _maxHealth;
    }

    // Getters and Setters
    public float Health {
        get { return this._health; }
        set { this._health = value; }
    }
    public float MaxHealth { get => this._maxHealth; }

    public float Speed { get => this._speed; }

    public AttackInfo AttackInfo { 
        get => this._attackType; 
        set => this._attackType = value;    
    }

    public EntityController Controller { get => this._controller; }

}
