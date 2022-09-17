using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(-3, 10, -3);
    }

    private void LateUpdate()
    {
        Vector2 scrollDelta = Input.mouseScrollDelta;

        if (scrollDelta.y < 0 && offset.y > 3 || scrollDelta.y > 0 && offset.y < 10)
        {
            offset = new Vector3(offset.x, offset.y + scrollDelta.y * 0.3f, offset.z);
        }

        transform.position = target.position + offset;
        transform.LookAt(target.transform);
    }

}
