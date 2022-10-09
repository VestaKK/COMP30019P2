using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHandler
{
    private EntityController _entity;

    // We'll animate the player using 2D blend tree, so we'll need the player's velocity
    private float _velocityY;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _relativeVelocity = Vector3.zero;

    [SerializeField] float _speed;
    [SerializeField] float _gravity;

    // Controls smooth turning
    float rotationSpeed;
    float rotationTime = 0.05f;

    // timeSinceGrounded is a debug variable
    public float timeSinceGrounded;

    public MotionHandler(EntityController entity) {
        this._entity = entity;
    }

    public void UpdateVelocity()
    {
        // Recalculate velocity every frame
        _velocity = Vector3.zero;

        Vector3 direction = _entity.CalculateMoveDirection();

        // Calculate the correct movement angle relative to the Camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;

        // Prevents random movement drift due to floating point stuff
        if (direction.magnitude >= 0.1f)
        {
            _velocity = moveDir;
        }

        if (_entity.Controller.isGrounded)
        {
            // Make sure controller will be sent into the ground
            // Otherwise controller won't be grounded
            _velocityY = -2.0f;
            timeSinceGrounded = 0;
        }
        else
        {
            timeSinceGrounded += Time.deltaTime;
            _velocityY -= _gravity * Time.deltaTime;
        }

        _velocity.y = _velocityY;
    }
    public void UpdateRelativeVelocity() {
        Quaternion transformRotation = Quaternion.FromToRotation(_entity.transform.forward, Vector3.forward);
        _relativeVelocity = transformRotation * _velocity;
    }

    public float GetTurnAngle(float targetAngle) {
        return Mathf.SmoothDampAngle(_entity.transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);
    }

    // Getters and Setters
    public Vector3 RelativeVelocity { 
        get { return _relativeVelocity; }
        set { _relativeVelocity = new Vector3(value.x, value.y, value.z); }
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

    public float Speed { get { return _speed; } }

    public Vector3 Velocity {
        get { return _velocity; }
        set { _velocity = new Vector3(value.x, value.y, value.z); }
    }

    public void GravityOnly()
    {
        _entity.Controller.Move(new Vector3(0, _velocity.y, 0));
    }

    public Camera Camera { get => _entity.Camera; }
}