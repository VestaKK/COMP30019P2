using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private MeleeHitboxController meleeHitbox;

    [SerializeField] private int damage = 20;
    [SerializeField] private float meleeSwingSpeed = 2f;
    [SerializeField] private float meleeLingerTime = 0.5f; 
    [SerializeField] private float meleeRange = 1.0f;
    [SerializeField] private float meleeReach = 1.0f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject parent; // gameObject we want as parent of MeleeHitbox
    private float swingTimer = 0;
    public bool isSwinging = false;

    // Update is called once per frame
    void Update()
    {
        if (swingTimer > 0)
        {
            swingTimer -= Time.deltaTime;
        }

        if (isSwinging && swingTimer <= 0 && meleeHitbox != null)
        {
            swingTimer = 1/meleeSwingSpeed;
            Transform parentTransform = parent.transform;
            MeleeHitboxController newMeleeSwing = Instantiate(
                meleeHitbox, 
                parentTransform.position + parentTransform.forward * meleeReach + offset, 
                parentTransform.rotation, 
                parentTransform) as MeleeHitboxController;
            newMeleeSwing.transform.localScale *= meleeRange;
            newMeleeSwing.damage = damage;
            newMeleeSwing.meleeLingerTime = meleeLingerTime;
        }
    }
}
