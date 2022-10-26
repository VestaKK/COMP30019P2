using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Mob : Entity, IDamageable
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    public UnityEvent LockedOn;
    public UnityEvent LockedOff;

    private AttackInfo _attackType;

    public AttackInfo AttackInfo { 
        get => this._attackType; 
        set => this._attackType = value;    
    }

    protected AudioSource audioSource;

    [SerializeField] protected AudioClip[] takeDamageClip;
    public abstract void TakeDamage(AttackInfo info);
    
    [SerializeField] protected AudioClip deathClip;
    public abstract void OnDeath();
    protected bool isDead = false;

    protected void Awake() {
        this._health = _maxHealth;
        this.audioSource = GetComponent<AudioSource>();
    }

    public void SetupHealthbar(Canvas canvas, Camera camera) {
        // HealthBar.transform.SetParent(canvas.transform);
        if(HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera)) {
            faceCamera._camera = camera;
        }
    }

    public void TakeDamage(int dmg) {
        TakeDamage(new AttackInfo(dmg, Vector3.zero,0,Vector3.zero));
    }


    // Getters and Setters
    public float Health {
        get { return this._health; }
        set { this._health = value; }
    }

    public MobController MobController { get => this.EntityController as MobController; }

    public float MaxHealth { get => this._maxHealth; set => this._maxHealth = value; }
    public ProgressBar HealthBar { get => this.MobController.HealthBar; set => this.MobController.HealthBar = value; }
}