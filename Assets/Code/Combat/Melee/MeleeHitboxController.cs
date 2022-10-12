using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxController : HitboxController
{


    // Event triggers on collission with a hurtbox. Hurtbox must have both a collider
    // and a rigidbody for this to work, which is why the hurtbox is a child of the parent entity,
    // because of the character controller having its own rigidbody and collider.
    // Also, hitbox must be on the hitbox layer and hurtbox must be on the hurtbox layer.
    public void OnTriggerEnter(Collider other)
    {
        // Since other is a Hurtbox typically this will work
        // However we should still put a check in here to see if the collider belongs to a Hurtbox
        IDamageable damageable = other.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
        if (damageable != null) 
        {
            damageable.TakeDamage(attackInfo);
        }
    }
}
