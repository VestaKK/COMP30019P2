using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    private Player player;
    // duration and counter for player flashing when damaged
    public float flashDuration;
    private float flashCounter = 0;
    private Renderer rend;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
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
        player.TakeDamage(damage);
        player.Health -= damage;
        flashCounter = flashDuration;
        rend.material.SetColor("_Color", Color.white);
    }
}
