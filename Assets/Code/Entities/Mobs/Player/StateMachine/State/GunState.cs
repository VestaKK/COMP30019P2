using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunState : PlayerState
{
    public GunState(StateManager stateManager)
        : base(stateManager) {}

    public override void Enter()
    {
        Player.LookAtMouse();
        Player.Motion.UpdateRelativeVelocity();
        HandleMovementAnimations();
    }

    public override void Update()
    {
        Player.LookAtMouse();

        Player.Motion.UpdateRelativeVelocity();

        if (Input.GetMouseButtonDown(0)) 
        {
            if (PlayerInventory.HasBullets())
            {
                (Player.Entity as Player).ShootBullet();
                PlayerInventory.FireBullet();
            }   
        }


        

        HandleMovementAnimations();

        Player.EntityMove();
    }

    void HandleMovementAnimations()
    {
        Player.Animator.SetFloat("RelativeVelocityX", Player.RelativeVelocity.x);
        Player.Animator.SetFloat("RelativeVelocityZ", Player.RelativeVelocity.z);
    }

    public override void Exit()
    {
        Player.Motion.UpdateRelativeVelocity();
        HandleMovementAnimations();
    }

    public override void CheckSwitchStates()
    {
        if (!InputManager.GetKey(InputAction.Aim)) 
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
        else if (InputManager.GetKeyDown(InputAction.Roll))
        {
            _stateManager.SwitchState(_stateManager.Roll());
        }
    }
}
