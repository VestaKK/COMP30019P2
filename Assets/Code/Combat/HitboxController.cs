using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitboxController : MonoBehaviour {

    protected AttackInfo attackInfo;
    protected float duration;
    protected string tag;
    // We send over a packet of information from the hitbox to the hurtbox.
    // Can contain basically any data that we care about i.e damage,
    // knockback vector, effects etc Hurtbox will take care of how these parameters
    // affect the entity

    public void Initialize(AttackInfo attack, float duration, string gameObjectTag)
    {
        this.attackInfo = attack;
        this.duration = duration;
        this.tag = gameObjectTag;
    }

    // We can change this latet but it works for now
    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}