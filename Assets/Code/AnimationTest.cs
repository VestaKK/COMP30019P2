using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            animator.SetBool("Kick1", true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetBool("Kick1", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("DeathAnimation");
        }
    }
}
