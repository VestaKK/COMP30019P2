using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField] private MotionHandler motionHandler;

    [SerializeField] protected CharacterController controller;
    [SerializeField] protected Camera camera;
    [SerializeField] protected Animator animator;

    public abstract Vector3 CalculateMoveDirection();

    void Start() {
        controller.enabled = true;
    }
    
    protected void LookInDirection(float angle) {
            transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    protected void LookAtMouse()
    {
        // Construct a plane that is level with the player position
        Plane playerPlane = new Plane(Vector3.up, controller.center);

        // Fire a ray from the mouse screen position into the world
        Ray mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

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

    protected void LookAtTarget(Transform target)
    {
        // Create Vector from Player to the target
        Vector3 P = transform.position;
        Vector3 T = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 P2T = (T - P).normalized;

        // Find angles in degrees needed to face the target
        float targetAngle = Mathf.Atan2(P2T.x, P2T.z) * Mathf.Rad2Deg;

        // Rotate player towards the target
        // Ensures the player will face the target directly when given a small turning angle
        if (Vector3.Dot(P2T, transform.forward) < 0.95)
        {
            float turnAngle = MotionHandler.GetTurnAngle(targetAngle);
            LookInDirection(turnAngle);
        }
        else
        {
            MotionHandler.RotationSpeed = 0;
            LookInDirection(targetAngle);
        }
    }

    protected void LookAtMovementDirection() { 
        float targetAngle = Mathf.Atan2(Velocity.x, Velocity.z) * Mathf.Rad2Deg;
        float turnAngle = MotionHandler.GetTurnAngle(targetAngle);
        LookInDirection(turnAngle);
    }

    // Getters and Setters
    public Animator Animator { get { return this.animator; } }
    public MotionHandler MotionHandler { get { return this.motionHandler; } }

    public float Speed { get { return MotionHandler.Speed; } }

    public Vector3 Velocity { 
        get { return MotionHandler.Velocity; }
        set { MotionHandler.Velocity = value; } 
    }

    public Camera Camera { get { return this.camera; } }
    public CharacterController Controller { get { return this.controller; } }

}