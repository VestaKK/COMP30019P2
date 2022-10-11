using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public Enemy() : base() {}
    public override void TakeDamage(AttackHitInfo info) {

        Health -= info.damage;
        if (Health <= 0)
        {
            Debug.Log("Dead");
            // OnDeath();
        }
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject);
        // die
    }
}