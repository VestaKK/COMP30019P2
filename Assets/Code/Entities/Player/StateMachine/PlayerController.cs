using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{

    // Player's melee controller
    [SerializeField] MeleeController playerMelee;

    // TODO: Change to an ENEMY transform
    [SerializeField] bool _isRolling = false;

    StateManager _stateManager;
    PlayerState _currentState;

    private void Awake()
    {
        base.Awake();
        _stateManager = new StateManager(this, StateManager.State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        Motion.UpdateVelocity();
        StateManager.Update();
    }

    // Called by rolling animation
    void AnimationEndRoll()
    {
        _isRolling = false;
    }

    public void PlayerMove()
    {
        Velocity = Velocity * Motion.Speed;//new Vector3(Motion.Speed * Velocity.x, Velocity.y, Motion.Speed * Velocity.z);
        Controller.Move(Velocity * Time.deltaTime);
    }

    public override Vector3 CalculateMoveDirection()
    {
        float left = InputManager.instance.GetKey(InputAction.Left) ? -1.0f : 0;
        float right = InputManager.instance.GetKey(InputAction.Right) ? 1.0f : 0;
        float forward = InputManager.instance.GetKey(InputAction.Forward) ? 1.0f : 0;
        float back = InputManager.instance.GetKey(InputAction.Back) ? -1.0f : 0;

        // Calculate object space move direction
        float horizontal = left + right;
        float vertical = forward + back;
        return new Vector3(horizontal, 0, vertical).normalized;
    }


    // Getters and Setters
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public MeleeController PlayerMelee { get => playerMelee; set => playerMelee = value; }
    public StateManager StateManager { get => _stateManager; set => _stateManager = value; }

    public Vector3 Velocity { get => Motion.Velocity; set => Motion.Velocity = value; }
}
