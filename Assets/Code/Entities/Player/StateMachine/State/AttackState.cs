using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerContext playerContext, StateManager stateManager)
        : base(playerContext, stateManager) { }

    public override void Enter()
    {
        if (_ctx.LockOnTarget == null)
        {
            _ctx.LookAtMouse();
        }
        else
        {
            _ctx.LookAtTarget(true);
        }

        _ctx.PlayerMelee.OnClick();

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (InputManager.instance.GetKeyDown(InputAction.Attack)) 
        {
            if (_ctx.LockOnTarget == null)
            {
                _ctx.LookAtMouse();
            }
            else
            {
                _ctx.LookAtTarget();
            }
            _ctx.PlayerMelee.OnClick();
        }

        _ctx.GravityOnly();
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.PlayerMelee.isResting && !_ctx.PlayerMelee.isAttacking) 
        {
            if (InputManager.instance.GetKeyDown(InputAction.Roll))
            {
                _stateManager.SwitchState(_stateManager.Roll());
            }
            else if (!(_ctx.Velocity.x == 0 && _ctx.Velocity.z == 0)) 
            {
                _stateManager.SwitchState(_stateManager.Walk());
            }
            else
            {
                _stateManager.SwitchState(_stateManager.Idle());
            }
        }
    }
}
