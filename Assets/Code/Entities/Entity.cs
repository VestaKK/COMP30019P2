using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    protected EntityController _controller;

    [SerializeField] float _speed;
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    [SerializeField] private AttackHitInfo damage;

    public Entity() {
        this._health = _maxHealth;
    }

    public abstract void TakeDamage(AttackHitInfo info);
    public abstract void OnDeath();

    // Getters and Setters
    public float Health {
        get { return this._health; }
        set { this._health = value; }
    }

    public float Speed { get => this._speed; }
}