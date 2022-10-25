using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(StateManager stateManager)
        : base(stateManager) {}

    public override void Enter()
    {
        Player.Motion.UpdateRelativeVelocity();
        HandleMovementAnimations();
    }

    public override void Exit()
    {
        Player.RelativeVelocity = new Vector3(0, 0, 0);
        Player.Animator.SetFloat("RelativeVelocityX", Player.RelativeVelocity.x);
        Player.Animator.SetFloat("RelativeVelocityZ", Player.RelativeVelocity.z);
    }

    public override void Update()
    {
        if (Player.LockOnTarget != null)
        {
            Player.LookAtTarget();
        }
        else
        {
            Player.LookAtMovementDirection();
        }

        Player.Motion.UpdateRelativeVelocity();

        HandleMovementAnimations();

        Player.EntityMove();
    }

    void HandleMovementAnimations()
    {
        Player.Animator.SetFloat("RelativeVelocityX", Player.RelativeVelocity.x);
        Player.Animator.SetFloat("RelativeVelocityZ", Player.RelativeVelocity.z);
    }

    public override void CheckSwitchStates()
    {
        if (InputManager.GetKeyDown(InputAction.Roll))
        {
            _stateManager.SwitchState(_stateManager.Roll());
        }
        else if (InputManager.GetKeyDown(InputAction.Attack) &&
            !Player.PlayerMelee.IsResting)
        {
            _stateManager.SwitchState(_stateManager.Attack());
        }
        else if (!Player.IsMoving())
        {
            _stateManager.SwitchState(_stateManager.Idle());
        }
        else if (InputManager.GetKey(InputAction.Aim))
        {
            _stateManager.SwitchState(_stateManager.Gun());
        }
    }
}
