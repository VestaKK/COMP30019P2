using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAnimationTest : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            animator.SetBool("isShooting", true);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            animator.SetBool("isShooting", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("DeathAnimation");
        }
    }
}
