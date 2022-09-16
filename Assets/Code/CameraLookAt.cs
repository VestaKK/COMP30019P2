using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(-3, 9, -3);
    }

    private void LateUpdate()
    {
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform);
    }

}
