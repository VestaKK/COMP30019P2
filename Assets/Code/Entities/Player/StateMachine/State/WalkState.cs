using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(StateManager stateManager)
        : base(stateManager) {}

    public override void Enter()
    {
        if (Player.LockOnTarget != null)
        {
            Player.LookAtTarget();
        }
        else
        {
            Player.LookAtMovementDirection();
        }

        HandleMovementAnimations();

        Player.Velocity = new Vector3(Player.Speed * Player.Velocity.x, Player.Velocity.y, Player.Speed * Player.Velocity.z);
        Player.Controller.Move(Player.Velocity * Time.deltaTime);
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
    }
}
