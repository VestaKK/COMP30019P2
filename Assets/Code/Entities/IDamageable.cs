using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    public void TakeDamage(AttackHitInfo info);
    public void OnDeath();
}