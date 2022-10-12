using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected EntityController _controller;

    [SerializeField] float _speed;
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    private AttackInfo _attackType;

    public abstract void TakeDamage(AttackInfo info);
    public abstract void OnDeath();

    protected void Awake() {
        this._health = _maxHealth;
    }

    public void SetupHealthbar(Canvas canvas, Camera camera) {
        HealthBar.transform.SetParent(canvas.transform);
        // if(HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera)) {
        //     faceCamera.Camera = camera;
        // }
    }

    public void TakeDamage(int dmg) {
        TakeDamage(new AttackInfo(dmg, Vector3.zero,0,Vector3.zero));
    }

    // Getters and Setters
    public float Health {
        get { return this._health; }
        set { this._health = value; }
    }
    public float MaxHealth { get => this._maxHealth; }

    public float Speed { get => this._speed; }

    public AttackInfo AttackInfo { 
        get => this._attackType; 
        set => this._attackType = value;    
    }

    public EntityController Controller { get => this._controller; }

    public ProgressBar HealthBar { get => this.Controller.HealthBar; }

}
