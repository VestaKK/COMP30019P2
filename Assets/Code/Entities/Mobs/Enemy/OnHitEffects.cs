using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffects : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] ParticleSystemRenderer particles;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.OnTakeDamage += HitParticles;
    }

    private void HitParticles()
    {
        Instantiate(particles, transform);
    }

    private void OnDisable()
    {
        _enemy.OnTakeDamage -= HitParticles;
    }
}
