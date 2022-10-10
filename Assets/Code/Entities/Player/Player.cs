using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;

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
        this._controller = this.GetComponent<PlayerController>();
    }

    public override void TakeDamage(int damage) {

        Health -= damage;
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