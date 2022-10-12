using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController: MonoBehaviour 
{
    [SerializeField] protected EntityController _entity;
    [SerializeField] protected HitboxController _hitbox;
    [SerializeField] protected Vector3 _offset;

    [SerializeField] protected int _maxAttacks;

    public bool isResting = false;
    public bool isAttacking = false;
    public float coolDown = 0;
    [SerializeField] protected float _maxCooldown;
    public int clickCount = 0;

    protected abstract void SpawnHitbox(AttackInfo info);

    // Should return whether or not an an animation has triggered
    protected abstract bool CheckAnimationTransitions();

    protected abstract void UpdateController();

    // OnClick returns the result of CheckAnimationTransitions
    public bool OnClick() {
        if (isResting) return false;

        clickCount++;

        clickCount = Mathf.Clamp(clickCount, 0, _maxAttacks);

        return CheckAnimationTransitions();
    }

    void Update()
    {
        UpdateController();

        if (coolDown <= 0)
        {
            if (isResting) 
            {
                clickCount = 0;
                isResting = false;
            }
            coolDown = 0;
        }
        else 
        {
            coolDown -= Time.deltaTime;
        }

        if (isAttacking) 
        {
            CheckAnimationTransitions();
        }
    }

    public void AttackAnimationEvent() 
    {
        SpawnHitbox(Controller.Entity.AttackInfo);
    }

    public EntityController Controller { get => this._entity; }
    public AttackInfo AttackInfo {
        get => Controller.Entity.AttackInfo;
        set => Controller.Entity.AttackInfo = value;
    }
}