using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : Mob
{

    public Enemy() : base() {}
    public override void TakeDamage(AttackInfo info) {

        Health -= info.Damage;
        if (Health <= 0)
        {
            OnDeath();
        }
        Debug.Log("HEALTHBAR: " + (Health / MaxHealth));
        #if UNITY_EDITOR
            EditorGUIUtility.PingObject(HealthBar);
        #endif
        HealthBar.SetProgress(Health / MaxHealth);
    }

    public override void OnDeath()
    {
        gameObject.SetActive(false);
        Destroy(HealthBar.gameObject);
        Destroy(this.gameObject);
        // die
    }
}