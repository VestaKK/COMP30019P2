using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffectsPlayer : MonoBehaviour
{
    private Player _player;
    [SerializeField] ParticleSystem particles;

    ParticleSystem currParticles;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _player = GetComponent <Player>();
        _player.OnTakeDamage += HitParticles;
    }

    private void HitParticles()
    { 
        if (currParticles != null)
            Destroy(currParticles.gameObject);

        currParticles = Instantiate(particles, transform.position + _player.EntityController.Controller.center, transform.rotation);
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
        _player.OnTakeDamage -= HitParticles;
    }
}
