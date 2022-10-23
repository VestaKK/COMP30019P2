using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffects : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] ParticleSystem particles;
    ParticleSystem currParticles;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.OnTakeDamage += HitParticles;
    }

    private void HitParticles()
    {
        if (currParticles != null)
            Destroy(currParticles.gameObject);

        currParticles = Instantiate(particles, transform.position + _enemy.EntityController.Controller.center, transform.rotation, transform);
    }

    private void Update()
    {
        if (currParticles == null) return;

        if (!currParticles.IsAlive())
        {
            Destroy(currParticles.gameObject);
        }
    }

    private void OnDisable()
    {
        _enemy.OnTakeDamage -= HitParticles;
    }
}
