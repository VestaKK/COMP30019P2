using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mob
{
    public delegate void DamageEvent();
    public event DamageEvent OnTakeDamage;

    public Enemy() : base() {}
    public override void TakeDamage(AttackInfo info) {

        Health -= info.Damage;
        OnTakeDamage.Invoke();
        if (Health <= 0)
        {
            OnDeath();
        }
        HealthBar.SetProgress(Health / MaxHealth);
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        Destroy(HealthBar.gameObject);
        Destroy(this.gameObject);
        // die
    }

    // Probably don't want to set
    public Player Player { get => GameManager.CurrentPlayer; set => GameManager.CurrentPlayer = value; }
}