using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // TODO: add LockOnTarget action
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 10f;
    [SerializeField] Transform camera;
    [SerializeField] Transform LockOnTarget = null;

    // Controls smooth turning
    float turnVelocity;
    float turnSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    void PlayerMove() {
        // TODO: Change to Keybinds
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate move direction, relative to a 0 degree rotation where facing positive Z axis is 0 degrees
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        if (direction.magnitude >= 0.1f)
        {
            // Get the correct movement angle relative to the camera in Degrees
            float moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

            // With no target, we turn the player in the direction of movement
            if (LockOnTarget == null)
            {
                // Smooth out turning angle over time. May or may not be necessary
                float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, moveAngle, ref turnVelocity, turnSpeed);
                controller.transform.rotation = Quaternion.Euler(0, turnAngle, 0);

                // Calculate move direction relative to the camera
                Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;

                // Move Character
                controller.Move(moveDir * speed * Time.deltaTime);
            }
            // Otherwise we face the target independantly of the direction of movement
            else
            {
                // Create Vector from Player to the target
                Vector3 P = new Vector3 (controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);
                Vector3 T = new Vector3 (LockOnTarget.position.x, controller.transform.position.y, LockOnTarget.position.z);
                Vector3 P2T = (T - P).normalized;

                // Find angles in degrees needed to face the target
                float targetAngle = Mathf.Atan2(P2T.x, P2T.z) * Mathf.Rad2Deg;

                // Condition ensures object will face the target directly when given a small turning angle
                if (Vector3.Dot(P2T, controller.transform.forward) < 0.95) {
                    float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);

                    // We turn the player accordingly
                    controller.transform.rotation = Quaternion.Euler(0, turnAngle, 0);
                } else {
                    turnVelocity = 0;
                    controller.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
                }

                // Calculate movement direction and move the player
                Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
                controller.Move(moveDir * speed * Time.deltaTime);
            }
        }
    }


}
