using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : AttackController
{
    protected override bool CheckAnimationTransitions() 
    {
        // TODO: Make this nicer
        if (clickCount == 1)
        {
            isAttacking = true;
            AttackInfo = new MeleeAttackInfo(1, 0.2f, new Vector3(2, 1, 2), 3, _offset);
            Controller.Animator.SetBool("Hit1", true);
            return true;
        }

        if (clickCount >= 2 &&
            Controller.GetAnimatorStateInfo(0).normalizedTime > 0.6f &&
            Controller.GetAnimatorStateInfo(0).normalizedTime < 0.9f &&
            Controller.GetAnimatorStateInfo(0).IsName("Melee Hit 1"))
        {
            AttackInfo = new MeleeAttackInfo(1, 0.2f, new Vector3(2, 1, 3), 3, _offset);
            Controller.Animator.SetBool("Hit1", false);
            Controller.Animator.SetBool("Hit2", true);
            return true;
        }

        return false;
    }

    protected override void SpawnHitbox(AttackInfo i)
    {
        MeleeAttackInfo info = i as MeleeAttackInfo;
        MeleeHitboxController newMeleeHitbox = Instantiate(
                _hitbox,
                transform.position + transform.forward * info.Reach + _offset,
                transform.rotation,
                transform) as MeleeHitboxController;
        newMeleeHitbox.transform.localScale = info.Aoe;
        newMeleeHitbox.Initialize(info, info.Duration);
    }

    // TODO: Make this nicer
    protected override void UpdateController()
    {

        if (Controller.GetAnimatorStateInfo(0).normalizedTime > 0.95f) {
            if(Controller.GetAnimatorStateInfo(0).IsName("Melee Hit 1")) {
                Controller.Animator.SetBool("Hit1", false);
                coolDown = _maxCooldown;
                isAttacking = false;
                isResting = true;
            }

            if(Controller.GetAnimatorStateInfo(0).IsName("Melee Hit 2"))
            {
                Controller.Animator.SetBool("Hit2", false);
                coolDown = _maxCooldown;
                isAttacking = false;
                isResting = true;
            }
        }
    }


}
