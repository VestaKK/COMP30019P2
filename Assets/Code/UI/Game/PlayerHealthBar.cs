using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Image HPimage;
    private int maxHealth = 100;
    private int currentHealth = 100;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentHealth != 0) TakeDamage(10);

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        HPimage.fillAmount = currentHealth / (float)maxHealth;
    }

    private void TakeDamage(int damage) { 
        currentHealth -= damage;
    }
}
