using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{

    public Player() : base() {}

    public override void TakeDamage(AttackInfo info) {
        Health -= info.Damage;
        HealthBar.SetProgress(Health / MaxHealth);
        if (Health <= 0)
        {
            OnDeath();
        }

    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        // Destroy(this.gameObject);
        // die
    }
}