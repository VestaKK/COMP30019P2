using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedHitboxController : HitboxController
{
    [SerializeField] float speed = 0f;
    private void Awake()
    {
        Debug.Log("I have awoken");

    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    public void OnTriggerEnter(Collider other)
    {
        // Since other is a Hurtbox typically this will work
        // However we should still put a check in here to see if the collider belongs to a Hurtbox

        // Dirty Fix
        Debug.Log("Hmmm");
        if (other.transform.parent.tag == "Enemy") return;

        IDamageable damageable = other.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;
        if (damageable != null)
        {
            damageable.TakeDamage(attackInfo);
            Destroy(this.gameObject);
        } 
    }
}
