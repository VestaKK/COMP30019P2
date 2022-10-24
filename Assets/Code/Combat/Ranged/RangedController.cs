using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : AttackController
{   
    [SerializeField] private AudioClip[] _attackClips;

    protected override bool CheckAnimationTransitions()
    {
        if (clickCount == 1)
        {
            if (_attackClips.Length > 0 && !IsAttacking)
                _audioSource.PlayOneShot(_attackClips[Random.Range(0, _attackClips.Length)], 0.25f);
            IsAttacking = true;
            AttackInfo = new AttackInfo(10, Vector3.one, 1, _offset);
            Controller.Animator.SetBool("isShooting", true);
            return true;
        }
        return false;
    }

    protected override void SpawnHitbox(AttackInfo info)
    {
        Debug.Log("Projectile Fired");
        RangedHitboxController newRangedHitbox = Instantiate(_hitbox, transform.position + transform.forward * info.Reach, transform.rotation) 
            as RangedHitboxController;
        newRangedHitbox.transform.rotation = transform.rotation;
        newRangedHitbox.Initialize(AttackInfo, 10, this.gameObject.tag);
    }

    protected override void UpdateController()
    {
        if (Controller.GetAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            if (Controller.GetAnimatorStateInfo(0).IsName("CharacterArmature|Shoot"))
            {
                Controller.Animator.SetBool("isShooting", false);
                coolDown = _maxCooldown;
                IsAttacking = false;
                IsResting = true;
            }
        }
    }
}
