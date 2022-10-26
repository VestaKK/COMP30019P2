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
        PlayerInventory.OnInventoryUpdate += UpdateMaxHealth;
        PlayerInventory.OnInventoryUpdate += UpdateDamageBoost;
        PlayerInventory.OnInventoryUpdate += PlayItemPickupClip;
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

    private void UpdateMaxHealth(ItemSlot itemSlot)
    {
        if (itemSlot.item.id == 0) // Health boost item
        {
            float healthPercentage = this.Health / this.MaxHealth;
            float healthBoost = 0.2f;
            this.MaxHealth *= (1 + healthBoost * itemSlot.count) / (1 - healthBoost + healthBoost * itemSlot.count);
            this.Health = this.MaxHealth * healthPercentage;
        }
    }

    private void UpdateDamageBoost(ItemSlot itemSlot)
    {
        if (itemSlot.item.id == 1)
        {
            float damageBoost = 0.15f;
            ((PlayerController) this._controller).PlayerMelee.DamageBoost += damageBoost;
        }
    }

    private void PlayItemPickupClip(ItemSlot itemSlot)
    {
        FindObjectOfType<AudioManager>().Play("ItemPickup");
    }

    void Update()
    {
        ManageMovementAudio();
    }

    private bool isPlayingFootsteps = false;
    private bool isPlayingRollAudio = false;

    private void ManageMovementAudio() 
    {
        PlayerController playerController = (PlayerController) MobController;
        if (playerController.IsMoving() && !playerController.IsRolling && !isPlayingFootsteps)
        {
            StartCoroutine(PlayFootsteps(playerController));
        }
        if (playerController.IsRolling && !isPlayingRollAudio)
        {
            StartCoroutine(PlayRollAudio(playerController));
        }
    }

    private IEnumerator PlayFootsteps(PlayerController controller) 
    {
        isPlayingFootsteps = true;
        while (controller.IsMoving() && !controller.IsRolling)
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");
            yield return new WaitForSeconds(0.3f);
        }
        isPlayingFootsteps = false;
    }

    private IEnumerator PlayRollAudio(PlayerController controller)
    {
        isPlayingRollAudio = true;
        FindObjectOfType<AudioManager>().Play("Roll");
        yield return new WaitForSeconds(1.2f);
        isPlayingRollAudio = false;
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        // Destroy(this.gameObject);
        // die
    }
}