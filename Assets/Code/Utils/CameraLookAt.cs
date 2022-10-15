using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private CharacterController playerController = null;
    [SerializeField] private Vector3 offsetDirection;
    [SerializeField] private float cameraZoom;



    private void Start()
    {
        cameraZoom = 15.0f;
        offsetDirection = new Vector3(-4, 5, -4).normalized;
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (playerController == null) 
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
            transform.position = playerController.transform.position + cameraZoom * offsetDirection;
            transform.LookAt(playerController.transform);
        }

        // Adjust Zoom
        Vector2 scrollDelta = Input.mouseScrollDelta;
        if (scrollDelta.y < 0 && cameraZoom > 6 || scrollDelta.y > 0 && cameraZoom < 15)
        {
            cameraZoom += scrollDelta.y;
        }

        // Follow Player 
        Vector3 targetPosition = playerController.transform.position + cameraZoom * offsetDirection;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, targetPosition, 0.01f);
        transform.position = lerpPosition;
    }
}
