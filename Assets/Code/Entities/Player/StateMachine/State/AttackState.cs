using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(StateManager stateManager)
        : base(stateManager) { }

    public override void Enter()
    {
        if (Player.LockOnTarget == null)
        {
            Player.LookAtMouse();
        }
        else
        {
            Player.LookAtTarget(true);
        }

        Player.PlayerMelee.OnClick();

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (InputManager.GetKeyDown(InputAction.Attack)) 
        {
            if (Player.LockOnTarget == null)
            {
                Player.LookAtMouse();
            }
            else
            {
                Player.LookAtTarget();
            }
            Player.PlayerMelee.OnClick();
        }

        Player.Motion.GravityOnly();
    }

    public override void CheckSwitchStates()
    {
        if (Player.PlayerMelee.isResting && !Player.PlayerMelee.isAttacking) 
        {
            if (InputManager.GetKeyDown(InputAction.Roll))
            {
                _stateManager.SwitchState(_stateManager.Roll());
            }
            else if (!(Player.Velocity.x == 0 && Player.Velocity.z == 0)) 
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
