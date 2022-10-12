using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public Enemy() : base() {}
    public override void TakeDamage(AttackInfo info) {

        Health -= info.Damage;
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
}