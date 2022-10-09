using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField] private MotionHandler _motionHandler;

    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected Animator _animator;

    [SerializeField] protected Transform _lockOnTarget = null;

    public abstract Vector3 CalculateMoveDirection();

    private Entity _entity;

    protected void Awake() {
        _motionHandler = new MotionHandler(this);
    }

    void Start() {
        _controller.enabled = true;
    }

    public void LookInDirection(float angle) {
            transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void LookAtMouse()
    {
        // Construct a plane that is level with the player position
        Plane playerPlane = new Plane(Vector3.up, _controller.center);

        // Fire a ray from the mouse screen position into the world
        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(mouseRay, out float distanceToPlane))
        {
            // Calculate hitpoint using ray and distance to plane
            Vector3 mouseHitPoint = mouseRay.GetPoint(distanceToPlane);
            Vector3 P2M = (mouseHitPoint - transform.position).normalized;

            // Rotate player accordingly
            float targetAngle = Mathf.Atan2(P2M.x, P2M.z) * Mathf.Rad2Deg;
            LookInDirection(targetAngle);
        }
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
        Velocity = Entity.Speed * Velocity; //new Vector3(Entity.Speed * Velocity.x, Velocity.y, Velocity.z);
        Controller.Move(Velocity * Time.deltaTime);
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

    public Camera Camera { get { return this._camera; } }
    public CharacterController Controller { get { return this._controller; } }

    public Transform LockOnTarget { get { return this._lockOnTarget; } }

    public Entity Entity { get => this._entity; set => this._entity = value; } 

}