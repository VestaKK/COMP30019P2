using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{
    public delegate void DamageEvent();
    public event DamageEvent OnTakeDamage;

    public delegate void HealthUpdateEvent(float healthPercentage);
    public event HealthUpdateEvent OnHealthUpdate;

    PostProcessing _postProcessingScript;

    private void Awake()
    {
        base.Awake();
        _postProcessingScript = Camera.main.GetComponent<PostProcessing>();
        OnHealthUpdate += _postProcessingScript.SetChromaticAbberationIntensity;
    }

    private void OnDisable()
    {
        OnHealthUpdate -= _postProcessingScript.SetChromaticAbberationIntensity;
    }

    [SerializeField] private PlayerInventory _inventory;

    public override void TakeDamage(AttackInfo info) {
        Health -= info.Damage;

        OnTakeDamage.Invoke();
        OnHealthUpdate.Invoke(Health / MaxHealth);

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