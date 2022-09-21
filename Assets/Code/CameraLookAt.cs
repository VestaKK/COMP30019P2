using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offsetDirection;
    [SerializeField] private float cameraZoom;

    private void Start()
    {
        cameraZoom = 15.0f;
        offsetDirection = new Vector3(-4, 7, -4).normalized;
        transform.position = target.position + cameraZoom * offsetDirection;
        transform.LookAt(target);
    }

    private void LateUpdate()
    {
        // Adjust Zoom
        Vector2 scrollDelta = Input.mouseScrollDelta;
        if (scrollDelta.y < 0 && cameraZoom > 6 || scrollDelta.y > 0 && cameraZoom < 15)
        {
            cameraZoom += scrollDelta.y;
        }
        
        // Follow Player 
        transform.position = target.position + cameraZoom * offsetDirection;
        transform.LookAt(target);
    }
}
