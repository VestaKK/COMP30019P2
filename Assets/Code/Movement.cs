using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // TODO: add LockOnTarget
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 7f;
    [SerializeField] Transform camera;
    [SerializeField] GameObject LockOnTarget = null;

    // Controls smooth turning
    float turnVelocity;
    float turnSpeed = 0.05f;

    // Update is called once per frame
    void Update()
    {
        // TODO: Change to Keybinds
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate move direction, relative to a 0 degree rotation where 
        // the positive Z axis is 0 degrees
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        if (direction.magnitude >= 0.1f) {

            // Get the correct turning angle relative to the camera in Degrees
            float turnAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

            // Smooth out turning speed. May or may not be necessary
            if (LockOnTarget == null)
            {
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, turnAngle, ref turnVelocity, turnSpeed);
                controller.transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

                // Calculate move direction relative to the camera
                // Multiplication by Vector3.forward to turn Quaternion into Vector
                Vector3 moveDir = Quaternion.Euler(0f, turnAngle, 0f) * Vector3.forward;

                // Move Character
                controller.Move(moveDir * speed * Time.deltaTime);
            }
            else {
                controller.transform.LookAt(LockOnTarget.transform);

                // Calculate move direction relative to the camera
                // Multiplication by Vector3.forward to turn Quaternion into Vector
                Vector3 moveDir = Quaternion.Euler(0f, turnAngle, 0f) * Vector3.forward;

                // Move Character
                controller.Move(moveDir * speed * Time.deltaTime);
            }
        }
    }
}
