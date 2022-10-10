using UnityEngine;

public class MeleeHitboxController : MonoBehaviour
{
    
    public float meleeLingerTime = 0.3f;

    // We send over a packet of information from the hitbox to the hurtbox.
    // Can contain basically any data that we care about i.e damage,
    // knockback vector, effects etc Hurtbox will take care of how these parameters
    // affect the entity
    private AttackHitInfo hitInfo = new AttackHitInfo();

    public void Initialize(int damage, float lingerTime)
    {
        this.hitInfo.damage = damage;
        meleeLingerTime = lingerTime;
    }

    // We can change this latet but it works for now
    private void Update()
    {
        meleeLingerTime -= Time.deltaTime;
        if (meleeLingerTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Event triggers on collission with a hurtbox. Hurtbox must have both a collider
    // and a rigidbody for this to work, which is why the hurtbox is a child of the parent entity,
    // because of the character controller having its own rigidbody and collider.
    // Also, hitbox must be on the hitbox layer and hurtbox must be on the hurtbox layer.
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Hurtbox>(out Hurtbox hurtbox)) 
        {
            if (hurtbox != null) 
            {
                hurtbox.OnHit(hitInfo);
            }
        }
    }
}
