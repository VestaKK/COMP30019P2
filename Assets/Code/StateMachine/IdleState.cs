using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : PlayerState
{
    public IdleState(PlayerContext playerContext, StateManager stateManager)
        : base(playerContext, stateManager) {}

    public override void Enter()
    {
        _ctx.Velocity = new Vector3(0, 0, 0);
        _ctx.RelativeVelocity = new Vector3(0, 0, 0);
        _ctx.PlayerAnimator.SetFloat("RelativeVelocityX", _ctx.RelativeVelocity.x);
        _ctx.PlayerAnimator.SetFloat("RelativeVelocityZ", _ctx.RelativeVelocity.z);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (_ctx.LockOnTarget != null)
        {
            _ctx.LookAtTarget();
        }

        _ctx.GravityOnly();
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
        else if ( !(_ctx.Velocity.x == 0 && _ctx.Velocity.z == 0) ) 
        {
            _stateManager.SwitchState(_stateManager.Walk());
        }
    }
}
