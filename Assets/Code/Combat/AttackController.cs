using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController : MonoBehaviour 
{
    [SerializeField] protected MobController _mob;
    [SerializeField] protected HitboxController _hitbox;
    [SerializeField] protected Vector3 _offset;

    [SerializeField] protected int _maxAttacks;

    protected AudioSource _audioSource;

    private bool isResting = false;
    private bool isAttacking = false;
    protected float coolDown = 0;
    [SerializeField] protected float _maxCooldown;
    public int clickCount = 0;
    protected float damageBoost = 1;

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

    void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
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
        SpawnHitbox(Controller.Mob.AttackInfo);
    }

    public MobController Controller { get => this._mob; }
    public AttackInfo AttackInfo {
        get => Controller.Mob.AttackInfo;
        set => Controller.Mob.AttackInfo = value;
    }
    
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsResting { get => isResting; set => isResting = value; }

    public float DamageBoost { get => damageBoost; set => damageBoost = value; }
}