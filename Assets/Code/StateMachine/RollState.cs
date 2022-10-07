using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerState
{
    private const float INCREASED_TURN_TIME = 0.2f;
    private float _normalTurnTime = 0;

    public RollState(PlayerContext playerContext, StateManager stateManager)
        : base(playerContext, stateManager) {}

    public override void Enter()
    {
        if (!(_ctx.Velocity.x == 0 && _ctx.Velocity.z == 0))
        {
            float targetAngle = Mathf.Atan2(_ctx.Velocity.x, _ctx.Velocity.z) * Mathf.Rad2Deg;
            _ctx.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        _ctx.PlayerAnimator.applyRootMotion = true;
        _ctx.PlayerAnimator.SetTrigger("Roll");
        _ctx.IsRolling = true;
        _normalTurnTime = _ctx.TurnTime;
        _ctx.TurnTime = INCREASED_TURN_TIME;
    }

    public override void Exit()
    {
        _ctx.PlayerAnimator.applyRootMotion = false;
        _ctx.TurnTime = _normalTurnTime;
    }

    public override void Update()
    {
        if (_ctx.Velocity.x != 0 && _ctx.Velocity.z != 0)
        _ctx.LookAtMovementDirection();
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsRolling == false) 
        {
            if (!(_ctx.Velocity.x == 0 && _ctx.Velocity.z == 0))
            {
                _stateManager.SwitchState(_stateManager.Walk());
            }
            else if (_ctx.Velocity.x == 0 && _ctx.Velocity.z == 0) 
            { 
                _stateManager.SwitchState(_stateManager.Idle());
            }
        }
    }
}
