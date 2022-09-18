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
        offsetDirection = new Vector3(-3, 10, -3).normalized;
        camera = GetComponent<Camera>();
        cameraZoom = 15.0f;
        transform.position = target.position + cameraZoom * offsetDirection;
        transform.LookAt(target);
    }

    private void LateUpdate()
    {
        Vector2 scrollDelta = Input.mouseScrollDelta;

        if (scrollDelta.y < 0 && cameraZoom > 6 || scrollDelta.y > 0 && cameraZoom < 15)
        {
            cameraZoom += scrollDelta.y;
        }
        
        transform.position = target.position + cameraZoom * offsetDirection;
        transform.LookAt(target.position);
    }
}
