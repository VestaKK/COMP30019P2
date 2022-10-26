using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeelaMeleeController : AttackController
{
    protected override bool CheckAnimationTransitions()
    {
        if (clickCount == 1)
        {
            IsAttacking = true;
            AttackInfo = new MeleeAttackInfo(20, 0.2f, new Vector3(1f, 1f, 0.7f), 1, _offset);
            Controller.Animator.SetBool("Kick1", true);
            return true;
        }

        return false;
    }

    protected override void SpawnHitbox(AttackInfo i)
    {
        MeleeAttackInfo info = i as MeleeAttackInfo;
        MeleeHitboxController newMeleeHitbox = Instantiate(
                _hitbox,
                transform.position + transform.forward * info.Reach + transform.rotation *_offset,
                transform.rotation,
                transform) as MeleeHitboxController;
        newMeleeHitbox.transform.localScale = info.Aoe;
        newMeleeHitbox.Initialize(info, info.Duration, this.gameObject.tag);
    }

    protected override void UpdateController()
    {
        if (Controller.GetAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            if (Controller.GetAnimatorStateInfo(0).IsName("RobotArmature|Kick"))
            {
                Controller.Animator.SetBool("Kick1", false);
                coolDown = _maxCooldown;
                IsAttacking = false;
                IsResting = true;
            }
        }
    }
}
