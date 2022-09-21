using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: add LockOnTarget action
    [SerializeField] CharacterController controller;
    [SerializeField] Camera camera;
    [SerializeField] InputManager inputManager;

    // TODO: Change to an ENEMY transform
    // TODO: Find a way to lock on to targets using KeyBind
    [SerializeField] Transform LockOnTarget = null;

    [SerializeField] float speed;
    [SerializeField] float vertSpeed;
    [SerializeField] float gravity;
    [SerializeField] bool  hasJump = true;
    [SerializeField] float timeSinceGrounded;

    // Controls smooth turning
    float turnVelocity;
    float turnTime = 0.05f;

    void Start()
    {
        inputManager = InputManager.instance;
        controller.enabled = true;
    }

    void Update()
    {
        // Lock onto a Target
        if (inputManager.GetKeyDown(InputAction.LockOn))
        {
            // TODO: Create Enemies to actually lock onto LMAO
        }

        // Rotate the player
        if (Input.GetMouseButton(1))
        {
            LookAtMouse();
        }
        else if (LockOnTarget != null)
        {
            LookAtTarget();
        }

        // Move the Player
        PlayerMove();
    }

    void LookAtMouse() {

        // Construct a plane that is level with the player position
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Fire a ray from the mouse screen position into the world
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(mouseRay, out float distanceToPlane)) {

            // Calculate hitpoint using ray and distance to plane
            Vector3 mouseHitPoint = mouseRay.GetPoint(distanceToPlane);
            Vector3 P2M = (mouseHitPoint - transform.position).normalized;

            // Rotate player accordingly
            float targetAngle = Mathf.Atan2(P2M.x, P2M.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    void LookAtTarget() {

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

    void PlayerMove() {

        // Recalculate velocity every frame
        Vector3 velocity = Vector3.zero;

        // Process Input
        float left = inputManager.GetKey(InputAction.Left) ? -1.0f : 0;
        float right = inputManager.GetKey(InputAction.Right) ? 1.0f : 0;
        float forward = inputManager.GetKey(InputAction.Forward) ? 1.0f : 0;
        float back = inputManager.GetKey(InputAction.Back) ? -1.0f : 0;

        // Calculate object space move direction
        float horizontal = left + right;
        float vertical = forward + back;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate the correct movement angle relative to the camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;

        // Prevents random movement drift due to floating point stuff
        if (direction.magnitude >= 0.1f)
        {
            // Calculate movement direction and horizontal plane movement
            Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
            velocity = moveDir * speed;

            // We only turn in the movement direction if we haven't rotated the player already
            if (LockOnTarget == null && !Input.GetMouseButton(1))
            {
                // Smooth out turning angle over time.
                float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, moveAngle, ref turnVelocity, turnTime);
                transform.rotation = Quaternion.Euler(0, turnAngle, 0);
            }
        }

        // Vertical Speed and Jumping calculations (Probably going to be removed)
        if (controller.isGrounded)
        {
            vertSpeed = 0;
            timeSinceGrounded = 0;
            hasJump = true;
        }
        else 
        {
            timeSinceGrounded += Time.deltaTime;
        }

        if (timeSinceGrounded < 0.15f && hasJump)
        {
            vertSpeed = 0;
            if (inputManager.GetKey(InputAction.Jump))
            {
                hasJump = false;
                vertSpeed += 20.0f;
            }
        }
        else
        {
            hasJump = false;
            vertSpeed -= gravity * Time.deltaTime;
        }

        velocity.y = vertSpeed;
        controller.Move(velocity * Time.deltaTime);
    }
}
