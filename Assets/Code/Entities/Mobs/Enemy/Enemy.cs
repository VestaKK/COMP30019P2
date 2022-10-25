using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mob
{
    public delegate void DamageEvent();
    public event DamageEvent OnTakeDamage;
    public GameObject item;
    [SerializeField] private int _score;

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
        GameManager.AddToScore(_score);
        Destroy(HealthBar.gameObject);
        if (item != null)
            Instantiate(item, transform.position + Vector3.up, Quaternion.identity);
        Destroy(this.gameObject);
        // die
    }

    // Probably don't want to set
    public Player Player { get => GameManager.CurrentPlayer; set => GameManager.CurrentPlayer = value; }
}