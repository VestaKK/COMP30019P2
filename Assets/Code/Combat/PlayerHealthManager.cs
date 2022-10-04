using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int health;
    public int currentHealth;

    // duration and counter for player flashing when damaged
    public float flashDuration;
    private float flashCounter = 0;
    private Renderer rend;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
            gameObject.SetActive(false);

        if (flashCounter > 0)
        {
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                rend.material.SetColor("_Color", originalColor);  
            }
        }
    }

    public void HurtPlayer(int damage)
    {
        currentHealth -= damage;
        flashCounter = flashDuration;
        rend.material.SetColor("_Color", Color.white);
    }
}
