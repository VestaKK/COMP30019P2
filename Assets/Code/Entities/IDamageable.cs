using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    public void TakeDamage(AttackInfo info);
    public void OnDeath();
}