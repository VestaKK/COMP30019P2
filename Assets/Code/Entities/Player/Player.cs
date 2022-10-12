using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;

    public Player() : base() {}

    // Singleton Stuff
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        base.Awake();
    }

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