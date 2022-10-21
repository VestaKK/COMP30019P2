using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffectsPlayer : MonoBehaviour
{
    private Player _player;
    [SerializeField] ParticleSystemRenderer particles;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _player = GetComponent <Player>();
        _player.OnTakeDamage += HitParticles;
    }

    private void HitParticles()
    { 
        Instantiate(particles, transform.position + _player.EntityController.Controller.center, transform.rotation);
    }

    private void OnDisable()
    {
        _player.OnTakeDamage -= HitParticles;
    }
}
