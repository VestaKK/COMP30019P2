
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera _camera;

    private void Update()
    {
        if(_camera != null)
            transform.LookAt(_camera.transform, Vector3.up);
    }
}