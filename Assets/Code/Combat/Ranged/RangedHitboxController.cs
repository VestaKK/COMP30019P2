using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedHitboxController : HitboxController
{
    [SerializeField] float speed = 0f;

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
    
    // Should only detect Hurtboxes
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag.Equals(tag)) return;

        IDamageable damageable = other.gameObject.GetComponentInParent(typeof(IDamageable)) as IDamageable;

        if (damageable != null)
        {
            damageable.TakeDamage(attackInfo);
            Destroy(this.gameObject);
        } 
    }
}
