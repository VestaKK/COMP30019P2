using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHandler
{
    private EntityController entity;

    // We'll animate the player using 2D blend tree, so we'll need the player's velocity
    private float velocityY;
    private Vector3 velocity;
    private Vector3 relativeVelocity;

    private Camera camera;

    [SerializeField] float speed;
    [SerializeField] float gravity;

    // Controls smooth turning
    float rotationSpeed;
    float rotationTime = 0.05f;

    // timeSinceGrounded is a debug variable
    public float timeSinceGrounded;

    public MotionHandler(EntityController entity) {
        this.entity = entity;
        this.camera = entity.Camera;
    }

    public void UpdateVelocity()
    {
        // Recalculate velocity every frame
        velocity = Vector3.zero;

        Vector3 direction = entity.CalculateMoveDirection();

        // Calculate the correct movement angle relative to the camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;

        // Prevents random movement drift due to floating point stuff
        if (direction.magnitude >= 0.1f)
        {
            velocity = moveDir;
        }

        if (entity.Controller.isGrounded)
        {
            // Make sure controller will be sent into the ground
            // Otherwise controller won't be grounded
            velocityY = -2.0f;
            timeSinceGrounded = 0;
        }
        else
        {
            timeSinceGrounded += Time.deltaTime;
            velocityY -= gravity * Time.deltaTime;
        }

        velocity.y = velocityY;
    }
    public void UpdateRelativeVelocity() {
        Quaternion transformRotation = Quaternion.FromToRotation(entity.transform.forward, Vector3.forward);
        relativeVelocity = transformRotation * velocity;
    }

    public float GetTurnAngle(float targetAngle) {
        return Mathf.SmoothDampAngle(entity.transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);
    }

    // Getters and Setters
    public Vector3 RelativeVelocity { 
        get { return relativeVelocity; }
        set { relativeVelocity = new Vector3(value.x, value.y, value.z); }
    }
    public float RotationSpeed 
    { 
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    public float RotationTime
    {
        get { return rotationTime; }
        set { rotationTime = value; }
    }

    public float Speed { get { return speed; } }

    public Vector3 Velocity {
        get { return velocity; }
        set { velocity = new Vector3(value.x, value.y, value.z); }
    }
}