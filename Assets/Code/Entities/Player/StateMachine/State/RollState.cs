using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerState
{
    private const float INCREASED_TURN_TIME = 0.2f;
    private float _normalTurnTime = 0;

    public RollState(StateManager stateManager)
        : base(stateManager) {}

    public override void Enter()
    {
        if (Player.IsMoving())
        {
            float targetAngle = Mathf.Atan2(Player.Velocity.x, Player.Velocity.z) * Mathf.Rad2Deg;
            Player.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        Player.Animator.applyRootMotion = true;
        Player.Animator.SetTrigger("Roll");
        Player.IsRolling = true;
        _normalTurnTime = Player.Motion.RotationTime;
        Player.Motion.RotationTime = INCREASED_TURN_TIME;
    }

    public override void Exit()
    {
        Player.Animator.applyRootMotion = false;
        Player.Motion.RotationTime = _normalTurnTime;
    }

    public override void Update()
    {
        if (Player.Velocity.x != 0 && Player.Velocity.z != 0)
        Player.LookAtMovementDirection();
    }

    public override void CheckSwitchStates()
    {
        if (Player.IsRolling == false) 
        {
            if (Player.IsMoving())
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
