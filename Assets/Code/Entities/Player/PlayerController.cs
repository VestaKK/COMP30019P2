using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{

    // Player's melee controller
    [SerializeField] MeleeController playerMelee;
    [SerializeField] protected Camera _camera;

    // TODO: Change to an ENEMY transform
    [SerializeField] bool _isRolling = false;

    StateManager _stateManager;
    PlayerState _currentState;

    private void Awake()
    {
        base.Awake();
        _stateManager = new StateManager(this, StateManager.State.Idle);
        Entity = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        StateManager.Update();
    }

    // Called by rolling animation
    void AnimationEndRoll()
    {
        _isRolling = false;
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


    public override Vector3 CalculateMoveDirection()
    {
        bool iLeft = InputManager.GetKey(InputAction.Left);
        bool iRight = InputManager.GetKey(InputAction.Right);
        bool iBack = InputManager.GetKey(InputAction.Back);
        bool iForward = InputManager.GetKey(InputAction.Forward);
        bool isMoving = iForward || iRight || iBack || iLeft;

        // Prevents random movement drift due to floating point stuff
        if(!isMoving) {
            return new Vector3(0,0,0);
        }

        float left = iLeft ? -1.0f : 0;
        float right = iRight ? 1.0f : 0;
        float forward = iForward ? 1.0f : 0;
        float back = iBack ? -1.0f : 0;
        float horizontal = left + right;
        float vertical = forward + back;
        
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate the correct movement angle relative to the Camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
        return moveDir;
    }


    // Getters and Setters
    public Camera Camera { get { return this._camera; } }
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public MeleeController PlayerMelee { get => playerMelee; set => playerMelee = value; }
    public StateManager StateManager { get => _stateManager; set => _stateManager = value; }

    public Vector3 Velocity { get => Motion.Velocity; set => Motion.Velocity = value; }
}
