using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobController : EntityController
{

    [SerializeField] protected Transform _lockOnTarget = null;

    [SerializeField] protected ProgressBar _healthBar;

    protected void Awake() {
        base.Awake();
    }

    protected void Update() {
        base.Update();
    }

    public void LockOn(Mob other) {
        if(other == null) {
            _lockOnTarget = null;
            return;
        }
        _lockOnTarget = other.transform;
    }

    public void LookAtTarget(bool instant = false) {
        LookAtTarget(_lockOnTarget, instant);
    }
    
    public void LookAtTarget(Transform target, bool instant = false)
    {
        // Create Vector from Player to the target
        Vector3 P = transform.position;
        Vector3 T = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 P2T = (T - P).normalized;

        // Find angles in degrees needed to face the target
        float targetAngle = Mathf.Atan2(P2T.x, P2T.z) * Mathf.Rad2Deg;

        // Rotate player towards the target
        // Ensures the player will face the target directly when given a small turning angle
        if (Vector3.Dot(P2T, transform.forward) < 0.95 && !instant)
        {
            float turnAngle = Motion.GetTurnAngle(targetAngle);
            LookInDirection(turnAngle);
        }
        else
        {
            Motion.RotationSpeed = 0;
            LookInDirection(targetAngle);
        }
    }
    
    // Getters and Setters
    public Mob Mob { get => this.Entity as Mob; }

    public Transform LockOnTarget { get { return this._lockOnTarget; } }
    public float Health { get => this.Mob.Health; set => this.Mob.Health = value; }
    public float MaxHealth {get => this.Mob.MaxHealth; }
    public ProgressBar HealthBar { get => this._healthBar; }

    public bool IsLockedOn() { return LockOnTarget != null; }
}