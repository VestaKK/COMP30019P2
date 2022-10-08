using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{

    // We'll animate the player using 2D blend tree, so we'll need the player's velocity
    public float velocityY;
    public Vector3 velocity;
    public Vector3 relativeVelocity;

    [SerializeField] float speed;
    [SerializeField] float gravity;

    // Controls smooth turning
    float turnVelocity;
    float turnTime = 0.05f;

    public void CalculateRelativeVelocity() {
        Quaternion transformRotation = Quaternion.FromToRotation(entity.transform.forward, Vector3.forward);
        relativeVelocity = transformRotation * velocity;
    }

}