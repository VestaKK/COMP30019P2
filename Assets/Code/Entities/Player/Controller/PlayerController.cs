using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
A controller that implements on top of the EntityController to implement Player-specific methods
*/
public class PlayerController : EntityController
{
    // TODO: Change to an ENEMY transform
    // TODO: Find a way to lock on to targets using KeyBind
    [SerializeField] Transform LockOnTarget = null;
    [SerializeField] float attackMovementSpeedModifier = 0.6f;

    // timeSinceGrounded is a debug variable
    public float timeSinceGrounded;
    public bool isRolling = false;

    // Player's melee controller
    [SerializeField] MeleeController playerMelee;

    // TODO: Use a coroutine for animation states or something
    private void Update()
    {
        CalculateVelocity();
        
        RotateTransform();

        CalculateRelativeVelocity();

        HandleAttack();

        HandleMovementAnimations();

        PlayerMove();
    }

    void HandleMovementAnimations() {
        playerAnimator.SetFloat("RelativeVelocityX", relativeVelocity.x);
        playerAnimator.SetFloat("RelativeVelocityZ", relativeVelocity.z);
    }

    void PlayerMove() {

        if (InputManager.instance.GetKeyDown(InputAction.Roll) && !playerMelee.isAttacking && !isRolling)
        {
            if (velocity.x != 0 && velocity.z != 0) 
            {
                float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }

            playerAnimator.applyRootMotion = true;
            playerAnimator.SetTrigger("Roll");
            isRolling = true;
            turnTime = 0.2f;
        }
        else if (isRolling != true && !playerMelee.isAttacking)
        {
            velocity = new Vector3(speed * velocity.x, velocity.y, speed * velocity.z);
            controller.Move(velocity * Time.deltaTime);
        }
    }

    // Called by rolling animation
    void AnimationEndRoll() 
    {
        isRolling = false;
    }

    void HandleAttack()
    {
        if (playerMelee != null && !isRolling)
        {
            if (InputManager.instance.GetKeyDown(InputAction.Attack))
            {
                playerMelee.OnClick();
            }
        }
    }
}
