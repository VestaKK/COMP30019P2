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
    public bool isRolling = false;

    // Player's melee controller
    [SerializeField] MeleeController playerMelee;

    public PlayerController(Player player) : base(player) {

    }

    // TODO: Use a coroutine for animation states or something
    private void Update()
    {
        MotionHandler.UpdateVelocity();
        
        RotatePlayer();

        MotionHandler.UpdateRelativeVelocity();

        HandleAttack();

        HandleMovementAnimations();

        MovePlayer();
    }

    void HandleMovementAnimations() {
        animator.SetFloat("RelativeVelocityX", MotionHandler.RelativeVelocity.x);
        animator.SetFloat("RelativeVelocityZ", MotionHandler.RelativeVelocity.z);
    }

    void MovePlayer() {

        if (InputManager.instance.GetKeyDown(InputAction.Roll) && !playerMelee.isAttacking && !isRolling)
        {
            if (Velocity.x != 0 && Velocity.z != 0) 
            {
                float targetAngle = Mathf.Atan2(Velocity.x, Velocity.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }

            animator.applyRootMotion = true;
            animator.SetTrigger("Roll");
            isRolling = true;
            MotionHandler.RotationTime = 0.2f;
        }
        else if (isRolling != true && !playerMelee.isAttacking)
        {
            Velocity = new Vector3(Speed * Velocity.x, Velocity.y, Speed * Velocity.z);
            controller.Move(Velocity * Time.deltaTime);
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

    void RotatePlayer() {
        if (InputManager.instance.GetKeyDown(InputAction.Attack) && !playerMelee.isResting && LockOnTarget == null) {
            LookAtMouse();
        }
        else if (LockOnTarget != null && !isRolling)
        {
            LookAtTarget(LockOnTarget);
        }
        else if (Velocity.x != 0 && Velocity.z != 0 && !playerMelee.isAttacking)
        {
            LookAtMovementDirection();
        }
    }

    public override Vector3 CalculateMoveDirection() {
        // Process Input
        float left = InputManager.instance.GetKey(InputAction.Left) ? -1.0f : 0;
        float right = InputManager.instance.GetKey(InputAction.Right) ? 1.0f : 0;
        float forward = InputManager.instance.GetKey(InputAction.Forward) ? 1.0f : 0;
        float back = InputManager.instance.GetKey(InputAction.Back) ? -1.0f : 0;

        // Calculate object space move direction
        float horizontal = left + right;
        float vertical = forward + back;
        return new Vector3(horizontal, 0, vertical).normalized;
    }
}
