using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: add LockOnTarget action
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
    [SerializeField] Camera camera;
    [SerializeField] Transform LockOnTarget = null;

    // Controls smooth turning
    float turnVelocity;
    float turnSpeed = 0.02f;

    // Update is called once per frame
    void Update()
    {
        // Lock onto a Target
        if (InputManager.instance.GetKeyDown(KeyBindingAction.LockOn))
        {
            // TODO: Create Enemies to actually lock onto LMAO
        }

        // Rotate the player
        bool hasTurned = false;
        if (Input.GetMouseButton(1))
        {
            LookAtMouse();
            hasTurned = true;
        }
        else if (LockOnTarget != null)
        {
            LookAtTarget();
            hasTurned = true;
        }

        PlayerMove(hasTurned);
    }

    void LookAtMouse() {
        Plane playerPlane = new Plane(Vector3.up, controller.transform.position);
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(mouseRay, out float enter)) {

            Vector3 mouseHitPoint = mouseRay.GetPoint(enter);
            Vector3 P2M = (mouseHitPoint - controller.transform.position).normalized;

            float targetAngle = Mathf.Atan2(P2M.x, P2M.z) * Mathf.Rad2Deg;
            controller.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    void LookAtTarget() {

        // Create Vector from Player to the target
        Vector3 P = controller.transform.position;
        Vector3 T = new Vector3(LockOnTarget.position.x, controller.transform.position.y, LockOnTarget.position.z);
        Vector3 P2T = (T - P).normalized;

        // Find angles in degrees needed to face the target
        float targetAngle = Mathf.Atan2(P2T.x, P2T.z) * Mathf.Rad2Deg;

        // Condition ensures object will face the target directly when given a small turning angle
        if (Vector3.Dot(P2T, controller.transform.forward) < 0.95)
        {
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);
            controller.transform.rotation = Quaternion.Euler(0, turnAngle, 0);
        }
        else
        {
            turnVelocity = 0;
            controller.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

    } 

    void PlayerMove(bool hasTurned) {
        // TODO: Change to Keybinds

        float left = InputManager.instance.GetKey(KeyBindingAction.Left) ? -1.0f : 0;
        float right = InputManager.instance.GetKey(KeyBindingAction.Right) ? 1.0f : 0;
        float forward = InputManager.instance.GetKey(KeyBindingAction.Forward) ? 1.0f : 0;
        float back = InputManager.instance.GetKey(KeyBindingAction.Back) ? -1.0f : 0;

        float horizontal = left + right;
        float vertical = forward + back;

        // Calculate move direction, relative to a 0 degree rotation where facing positive Z axis is 0 degrees
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Get the correct movement angle relative to the camera in Degrees
        float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        
        if (direction.magnitude >= 0.1f)
        {
            // Calculate movement direction and move the player
            Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);

            // We only turn in the movement direction if
            // we haven't rotated the player already
            if (LockOnTarget == null && !hasTurned)
            {
                // Smooth out turning angle over time. May or may not be necessary
                float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, moveAngle, ref turnVelocity, turnSpeed);

                // Rotate the player accordingly
                controller.transform.rotation = Quaternion.Euler(0, turnAngle, 0);
            }
        }
    }


}
