using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: add LockOnTarget action
    [SerializeField] CharacterController controller;
    [SerializeField] Camera camera;
    [SerializeField] Transform LockOnTarget = null;
    [SerializeField] InputManager inputManager;

    [SerializeField] float speed;
    [SerializeField] float vertSpeed;
    [SerializeField] float gravity;
    [SerializeField] bool  hasJump = true;
    [SerializeField] float timeSinceGrounded;

    // Controls smooth turning
    float turnVelocity;
    float turnSpeed = 0.05f;

    void Start()
    {
        inputManager = InputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        controller.enabled = true;
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

        PlayerMove();
    }

    void LookAtMouse() {

        // Construct a plane that is level with the player position
        // Fire a ray from the mouse position into world space
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        // Fire a ray through the plane and see where it lands
        if (playerPlane.Raycast(mouseRay, out float distanceToPlane)) {

            // Calculate hitpoint using ray and distance to plane
            Vector3 mouseHitPoint = mouseRay.GetPoint(distanceToPlane);

            // direction vector from player to the mouse hit point
            Vector3 P2M = (mouseHitPoint - transform.position).normalized;

            // Rotate player accordinly
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
        
        if (Vector3.Dot(P2T, transform.forward) < 0.95)
        {
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0, turnAngle, 0);
        }
        // Ensures the player will face the target directly when given a small turning angle
        else
        {
            turnVelocity = 0;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    } 

    void PlayerMove() {

        Vector3 velocity = Vector3.zero;

        float left = inputManager.GetKey(InputAction.Left) ? -1.0f : 0;
        float right = inputManager.GetKey(InputAction.Right) ? 1.0f : 0;
        float forward = inputManager.GetKey(InputAction.Forward) ? 1.0f : 0;
        float back = inputManager.GetKey(InputAction.Back) ? -1.0f : 0;

        float horizontal = left + right;
        float vertical = forward + back;

        // Calculate world space move direction
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate the correct movement angle relative to the camera (Degrees)
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;

        // Prevents random drift due to floating point stuff
        if (direction.magnitude >= 0.1f)
        {
            // Calculate movement direction and move the player
            Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;

            // TODO: Decide on acceleration based or arcade style movement
            velocity = moveDir * speed;

            // We only turn in the movement direction if
            // we haven't rotated the player already
            if (LockOnTarget == null && !Input.GetMouseButton(1))
            {
                // Smooth out turning angle over time. May or may not be necessary
                float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, moveAngle, ref turnVelocity, turnSpeed);

                // Rotate the player accordingly
                transform.rotation = Quaternion.Euler(0, turnAngle, 0);
            }
        }

        // Vertical Speed calculations
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
