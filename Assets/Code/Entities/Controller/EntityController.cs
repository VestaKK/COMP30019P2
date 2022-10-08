using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private MotionController motionController;

    [SerializeField] CharacterController controller;
    [SerializeField] Camera camera;
    [SerializeField] Animator animator;


    void Start() {
        controller.enabled = true;
    }

    void RotateTransform() {
        if (InputManager.instance.GetKeyDown(InputAction.Attack) && !playerMelee.isResting && LockOnTarget == null) {
            LookAtMouse();
        }
        else if (LockOnTarget != null && !isRolling)
        {
            LookAtTarget();
        }
        else if (velocity.x != 0 && velocity.z != 0 && !playerMelee.isAttacking)
        {
            LookAtMovementDirection();
        }
    }

    public void LookAtMouse()
    {
        // Construct a plane that is level with the player position
        Plane playerPlane = new Plane(Vector3.up, controller.center);

        // Fire a ray from the mouse screen position into the world
        Ray mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(mouseRay, out float distanceToPlane))
        {
            // Calculate hitpoint using ray and distance to plane
            Vector3 mouseHitPoint = mouseRay.GetPoint(distanceToPlane);
            Vector3 P2M = (mouseHitPoint - transform.position).normalized;

            // Rotate player accordingly
            float targetAngle = Mathf.Atan2(P2M.x, P2M.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    void LookAtTarget()
    {
        // Create Vector from Player to the target
        Vector3 P = transform.position;
        Vector3 T = new Vector3(LockOnTarget.position.x, transform.position.y, LockOnTarget.position.z);
        Vector3 P2T = (T - P).normalized;

        // Find angles in degrees needed to face the target
        float targetAngle = Mathf.Atan2(P2T.x, P2T.z) * Mathf.Rad2Deg;

        // Rotate player towards the target
        // Ensures the player will face the target directly when given a small turning angle
        if (Vector3.Dot(P2T, transform.forward) < 0.95)
        {
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0, turnAngle, 0);
        }
        else
        {
            turnVelocity = 0;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    void LookAtMovementDirection() { 
        float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
        transform.rotation = Quaternion.Euler(0, turnAngle, 0);
    }

    void CalculateVelocity()
    {
        // Recalculate velocity every frame
        velocity = Vector3.zero;

        // Process Input
        float left = InputManager.instance.GetKey(InputAction.Left) ? -1.0f : 0;
        float right = InputManager.instance.GetKey(InputAction.Right) ? 1.0f : 0;
        float forward = InputManager.instance.GetKey(InputAction.Forward) ? 1.0f : 0;
        float back = InputManager.instance.GetKey(InputAction.Back) ? -1.0f : 0;

        // Calculate object space move direction
        float horizontal = left + right;
        float vertical = forward + back;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate the correct movement angle relative to the camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;

        // Prevents random movement drift due to floating point stuff
        if (direction.magnitude >= 0.1f)
        {
            velocity = moveDir;
        }

        if (controller.isGrounded)
        {
            // Make sure controller will be sent into the ground
            // Otherwise controller won't be grounded
            velocityY = -2.0f;
            timeSinceGrounded = 0;
        }
        else
        {
            timeSinceGrounded += Time.deltaTime;
            velocityY -= gravity * Time.deltaTime;
        }

        velocity.y = velocityY;
    }

    public Animator Animator { get { return animator; } }
    public MotionController MotionController { get { return motionController; } }

}