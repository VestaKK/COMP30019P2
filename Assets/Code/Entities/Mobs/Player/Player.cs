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

    public delegate void DeathEvent();
    public event DeathEvent OnPlayerInstanceDeath;

    private void Awake()
    {
        base.Awake();
        _postProcessingScript = Camera.main.GetComponent<PostProcessing>();
    }

    private void OnEnable()
    {
        OnHealthUpdate += _postProcessingScript.SetChromaticAbberationIntensity;
        PlayerInventory.OnInventoryUpdate += UpdateMaxHealth;
        PlayerInventory.OnInventoryUpdate += UpdateDamageBoost;
        PlayerInventory.OnInventoryUpdate += PlayItemPickupClip;
    }

    private void OnDisable()
    {
        OnHealthUpdate -= _postProcessingScript.SetChromaticAbberationIntensity;
        PlayerInventory.OnInventoryUpdate -= UpdateMaxHealth;
        PlayerInventory.OnInventoryUpdate -= UpdateDamageBoost;
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
            float healthBoost = 0.10f;
            this.MaxHealth *= (1 + healthBoost * itemSlot.count) / (1 - healthBoost + healthBoost * itemSlot.count);
            this.Health = this.MaxHealth * healthPercentage;
        }
    }

    private void UpdateDamageBoost(ItemSlot itemSlot)
    {
        if (itemSlot.item.id == 1)
        {
            float damageBoost = 0.10f;
            PlayerController controller = (PlayerController)this._controller;
            controller.PlayerMelee.DamageBoost += damageBoost;
            controller.BulletDamage *= 1.0f + damageBoost;
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

    // TODO: Move this to PlayerController
    public void ShootBullet() 
    {
        PlayerController controller = _controller as PlayerController;
        
        RangedHitboxController newRangedHitbox = Instantiate(controller.PlayerBullet, 
            transform.position + transform.forward * 1 + _controller.Controller.center, 
            transform.rotation) as RangedHitboxController;

        newRangedHitbox.Initialize(new AttackInfo(controller.BulletDamage, Vector3.one, 1, Vector3.zero), 10, this.gameObject.tag);
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        OnPlayerInstanceDeath.Invoke();
        // Destroy(this.gameObject);
        // die
    }
}