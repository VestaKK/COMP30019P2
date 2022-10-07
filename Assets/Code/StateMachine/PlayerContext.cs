using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CharacterController _controller;
    [SerializeField] Camera _camera;

    // TODO: Change to an ENEMY transform
    [SerializeField] Transform _lockOnTarget = null;
    [SerializeField] Animator _playerAnimator;

    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;

    // timeSinceGrounded is a debug variable
    [SerializeField] float _timeSinceGrounded;
    [SerializeField] bool _isRolling = false;

    [SerializeField] float _turnTime = 0.05f;

    private float _turnVelocity;

    // We'll animate the player using 2D blend tree, so we'll need the player's velocity
    [SerializeField] float _velocityY;
    [SerializeField] Vector3 _velocity;
    [SerializeField] Vector3 _relativeVelocity;

    // Player's melee controller
    [SerializeField] MeleeController playerMelee;

    StateManager _stateManager;
    PlayerState _currentState;

    private void Awake()
    {
        _stateManager = new StateManager(this);
        _currentState = _stateManager.Idle();
        _currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
        _currentState.Update();
        _currentState.CheckSwitchStates();
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
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    public void LookAtTarget(bool instant = false)
    {
        // Create Vector from Player to the target
        Vector3 P = transform.position;
        Vector3 T = new Vector3(_lockOnTarget.position.x, transform.position.y, _lockOnTarget.position.z);
        Vector3 P2T = (T - P).normalized;

        // Find angles in degrees needed to face the target
        float targetAngle = Mathf.Atan2(P2T.x, P2T.z) * Mathf.Rad2Deg;

        // Rotate player towards the target
        // Ensures the player will face the target directly when given a small turning angle
        if (Vector3.Dot(P2T, transform.forward) < 0.95 && !instant)
        {
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnTime);
            transform.rotation = Quaternion.Euler(0, turnAngle, 0);
        }
        else
        {
            _turnVelocity = 0;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    public void LookAtMovementDirection()
    {
        float targetAngle = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
        float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnTime);
        transform.rotation = Quaternion.Euler(0, turnAngle, 0);
    }

    public void CalculateVelocity()
    {
        // Recalculate velocity every frame
        _velocity = Vector3.zero;

        // Process Input
        float left = InputManager.instance.GetKey(InputAction.Left) ? -1.0f : 0;
        float right = InputManager.instance.GetKey(InputAction.Right) ? 1.0f : 0;
        float forward = InputManager.instance.GetKey(InputAction.Forward) ? 1.0f : 0;
        float back = InputManager.instance.GetKey(InputAction.Back) ? -1.0f : 0;

        // Calculate object space move direction
        float horizontal = left + right;
        float vertical = forward + back;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate the correct movement angle relative to the camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;

        // Prevents random movement drift due to floating point stuff
        if (direction.magnitude >= 0.1f)
        {
            _velocity = moveDir;
        }

        if (_controller.isGrounded)
        {
            // Make sure controller will be sent into the ground
            // Otherwise controller won't be grounded
            _velocityY = -2.0f;
            _timeSinceGrounded = 0;
        }
        else
        {
            _timeSinceGrounded += Time.deltaTime;
            _velocityY -= _gravity * Time.deltaTime;
        }

        _velocity.y = _velocityY;
    }

    public void CalculateRelativeVelocity()
    {
        Quaternion transformRotation = Quaternion.FromToRotation(transform.forward, Vector3.forward);
        _relativeVelocity = transformRotation * _velocity;
    }

    // Called by rolling animation
    void AnimationEndRoll()
    {
        _isRolling = false;
    }

    public void PlayerMove()
    {
        _velocity = new Vector3(_speed * _velocity.x, _velocity.y, _speed * _velocity.z);
        _controller.Move(_velocity * Time.deltaTime);
    }

    public void GravityOnly()
    {
        _controller.Move(new Vector3(0, _velocity.y, 0));
    }

    public PlayerState CurrentState { get => _currentState; set => _currentState = value; }
    public CharacterController Controller { get => _controller; set => _controller = value; }
    public Camera Camera { get => _camera; set => _camera = value; }
    public Transform LockOnTarget { get => _lockOnTarget; set => _lockOnTarget = value; }
    public Animator PlayerAnimator { get => _playerAnimator; set => _playerAnimator = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Gravity { get => _gravity; set => _gravity = value; }
    public float TimeSinceGrounded { get => _timeSinceGrounded; set => _timeSinceGrounded = value; }
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public float TurnVelocity { get => _turnVelocity; set => _turnVelocity = value; }
    public float TurnTime { get => _turnTime; set => _turnTime = value; }
    public float VelocityY { get => _velocityY; set => _velocityY = value; }
    public Vector3 Velocity { get => _velocity; set => _velocity = value; }
    public Vector3 RelativeVelocity { get => _relativeVelocity; set => _relativeVelocity = value; }
    public MeleeController PlayerMelee { get => playerMelee; set => playerMelee = value; }
    public StateManager StateManager { get => _stateManager; set => _stateManager = value; }
    public PlayerState CurrentState1 { get => _currentState; set => _currentState = value; }
}
