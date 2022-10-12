using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public Enemy() : base() {}
    public override void TakeDamage(AttackInfo info) {

        Health -= info.Damage;
        if(Controller.HealthBar != null) {
            Controller.HealthBar.UpdateHealthbar();
        }
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject);
        // die
    }
}