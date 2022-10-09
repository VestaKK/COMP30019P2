using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : PlayerState
{
    public IdleState(StateManager stateManager)
        : base(stateManager) {}

    public override void Enter()
    {
        Player.Animator.SetFloat("RelativeVelocityX", Player.RelativeVelocity.x);
        Player.Animator.SetFloat("RelativeVelocityZ", Player.RelativeVelocity.z);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (Player.LockOnTarget != null)
        {
            Player.LookAtTarget();
        }

        Player.Motion.GravityOnly();
    }

    public override void CheckSwitchStates()
    {
        if (InputManager.instance.GetKeyDown(InputAction.Roll))
        {
            _stateManager.SwitchState(_stateManager.Roll());
        }
        else if (InputManager.instance.GetKeyDown(InputAction.Attack) && 
            !Player.PlayerMelee.isResting)
        {
            _stateManager.SwitchState(_stateManager.Attack());
        }
        else if ( !(Player.Velocity.x == 0 && Player.Velocity.z == 0) ) 
        {
            _stateManager.SwitchState(_stateManager.Walk());
        }
    }
}
