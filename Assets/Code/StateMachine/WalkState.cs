using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerContext playerContext, StateManager stateManager)
        : base(playerContext, stateManager) {}

    public override void Enter()
    {
        if (_ctx.LockOnTarget != null)
        {
            _ctx.LookAtTarget();
        }
        else
        {
            _ctx.LookAtMovementDirection();
        }

        HandleMovementAnimations();

        _ctx.Velocity = new Vector3(_ctx.Speed * _ctx.Velocity.x, _ctx.Velocity.y, _ctx.Speed * _ctx.Velocity.z);
        _ctx.Controller.Move(_ctx.Velocity * Time.deltaTime);
    }

    public override void Exit()
    {
        _ctx.RelativeVelocity = new Vector3(0, 0, 0);
        _ctx.PlayerAnimator.SetFloat("RelativeVelocityX", _ctx.RelativeVelocity.x);
        _ctx.PlayerAnimator.SetFloat("RelativeVelocityZ", _ctx.RelativeVelocity.z);
    }

    public override void Update()
    {
        if (_ctx.LockOnTarget != null)
        {
            _ctx.LookAtTarget();
        }
        else
        {
            _ctx.LookAtMovementDirection();
        }

        _ctx.CalculateRelativeVelocity();

        HandleMovementAnimations();

        _ctx.PlayerMove();
    }

    void HandleMovementAnimations()
    {
        _ctx.PlayerAnimator.SetFloat("RelativeVelocityX", _ctx.RelativeVelocity.x);
        _ctx.PlayerAnimator.SetFloat("RelativeVelocityZ", _ctx.RelativeVelocity.z);
    }

    public override void CheckSwitchStates()
    {
        if (InputManager.instance.GetKeyDown(InputAction.Roll))
        {
            _stateManager.SwitchState(_stateManager.Roll());
        }
        else if (InputManager.instance.GetKeyDown(InputAction.Attack) && 
            !_ctx.PlayerMelee.isResting)
        {
            _stateManager.SwitchState(_stateManager.Attack());
        }
        else if ( _ctx.Velocity.x == 0 && _ctx.Velocity.z == 0 )
        {
            _stateManager.SwitchState(_stateManager.Idle());
        }
    }
}
