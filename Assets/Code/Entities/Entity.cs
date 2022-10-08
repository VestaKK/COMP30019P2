using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    protected EntityController controller;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    public abstract void TakeDamage(int damage);
    public abstract void OnDeath();

    // Getters and Setters
    public int Health {
        get { return this.health; }
        set { this.health = value; }
    }
}