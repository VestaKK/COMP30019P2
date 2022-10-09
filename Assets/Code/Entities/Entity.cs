using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    protected EntityController _controller;

    [SerializeField] float _speed;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;

    public abstract void TakeDamage(int damage);
    public abstract void OnDeath();

    // Getters and Setters
    public int Health {
        get { return this._health; }
        set { this._health = value; }
    }

    public float Speed { get => this._speed; }
}