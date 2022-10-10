using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    // Just to show that this works
    // hitInfo most likely needs to be tagged somehow
    // to avoid the player hitbox damaging its own hurtbox
    public void OnHit(AttackHitInfo hitInfo)
    {
        Debug.Log("Damage: " + hitInfo.damage.ToString());
    }
}
