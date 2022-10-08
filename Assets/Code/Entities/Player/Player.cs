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
            // Makes sure this isn't unloaded when loading a new scene
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public Player() {
        this.controller = new PlayerController(this);
    }

    public override void TakeDamage(int damage) {

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