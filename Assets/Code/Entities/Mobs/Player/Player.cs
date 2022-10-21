using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{
    public delegate void DamageEvent();
    public event DamageEvent OnTakeDamage;
    public Material _postProcessingMaterial;

    private void Awake()
    {
        base.Awake();
        HealthBar = UIManager.instance.GetComponentInChildren<ProgressBar>();
        _postProcessingMaterial = Camera.main.GetComponent<PostProcessing>().postProcessingMat;
        _postProcessingMaterial.SetFloat("_Amount", (1 - Health / MaxHealth) * 0.02f);
    }

    [SerializeField] private PlayerInventory _inventory;

    private PlayerInventory inventory;

    public override void TakeDamage(AttackInfo info) {
        Health -= info.Damage;

        OnTakeDamage.Invoke();

        _postProcessingMaterial.SetFloat("_Amount", (1 - Health/MaxHealth) * 0.02f);

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