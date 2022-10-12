using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField] private MotionHandler _motionHandler;

    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected Animator _animator;

    [SerializeField] protected Transform _lockOnTarget = null;

    public abstract Vector3 CalculateMoveDirection();

    private Entity _entity;
    private HealthBar _healthbar;

    protected void Awake() {
        _motionHandler = new MotionHandler(this);
    }

    protected void Update() {
        Motion.UpdateVelocity();
    }

    void Start() {
        _controller.enabled = true;
    }

    public void LookInDirection(float angle) {
            transform.rotation = Quaternion.Euler(0, angle, 0);
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
    public void LookAtMovementDirection() { 
        float targetAngle = Mathf.Atan2(Velocity.x, Velocity.z) * Mathf.Rad2Deg;
        float turnAngle = Motion.GetTurnAngle(targetAngle);
        LookInDirection(turnAngle);
    }
    public void EntityMove()
    {
        Velocity = Entity.Speed * Velocity;
        Controller.Move(Velocity * Time.deltaTime);
    }

    public bool IsMoving() {
        return (Velocity.x != 0 || Velocity.z != 0);
    }

    public AnimatorStateInfo GetAnimatorStateInfo(int index) { 
        return Animator.GetCurrentAnimatorStateInfo(index); 
    }

    // Getters and Setters
    public Animator Animator { get { return this._animator; } }
    public MotionHandler Motion { get => this._motionHandler; }

   public float Speed { get { return Entity.Speed; } }

    public Vector3 Velocity { 
        get { return Motion.Velocity; }
        set { Motion.Velocity = value; } 
    }
    public Vector3 RelativeVelocity { 
        get { return Motion.RelativeVelocity; }
        set { Motion.RelativeVelocity = value; } 
    }

    public CharacterController Controller { get { return this._controller; } }

    public Transform LockOnTarget { get { return this._lockOnTarget; } }

    public Entity Entity { get => this._entity; set => this._entity = value; }
    public float Health { get => this.Entity.Health; set => this.Entity.Health = value; }
    public float MaxHealth {get => this.Entity.MaxHealth; }

    public HealthBar HealthBar { get => this._healthbar; set => this._healthbar = value; }

}