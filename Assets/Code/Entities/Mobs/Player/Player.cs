using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{
    [SerializeField] private PlayerInventory _inventory;

    public static Player instance;

    private PlayerInventory inventory;

    public Player() : base() {}

    // Singleton Stuff
    private void InitSingleton() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        inventory = GetComponent<PlayerInventory>();
    }

    private void Awake()
    {
        InitSingleton();
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