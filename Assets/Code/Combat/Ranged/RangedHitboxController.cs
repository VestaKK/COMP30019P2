using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedHitboxController : HitboxController
{
    [SerializeField] float speed = 0f;
    [SerializeField] GameObject explosionParticles;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionParticles, transform.position, transform.rotation);
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
            Instantiate(explosionParticles,transform.position, transform.rotation);
        } 
    }
}
