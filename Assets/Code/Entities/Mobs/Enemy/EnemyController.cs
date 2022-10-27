using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MobController
{
    [SerializeField] float DetectionDistanceSq = 0.0f;
    [SerializeField] float AttackDistanceSq = 0.0f;
    [SerializeField] float ChaseDistanceSq = 0.0f;
    [SerializeField] AttackController enemyAttack;
    [SerializeField] bool isChasing;

    [SerializeField] protected AudioClip moveClip;
    [SerializeField] private AudioClip[] _attackClips;

    private AudioSource _audioSource;

    private void Awake()
    {
        base.Awake();
        Entity = this.GetComponent<Enemy>();
        agent.speed = Entity.Speed;

        _audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) return;
        base.Update();

        if (Mob.isDead)
        {
            agent.SetDestination(Enemy.Position);
            transform.LookAt(transform);
        }
        else
        {
            EntityMove();
        }
        // Check Attack
    }

    public void EntityMove() {
        if ((isChasing = ShouldChase()) && !enemyAttack.IsAttacking)
        {
            LookAtTarget(Enemy.Player.transform);
            agent.SetDestination(Enemy.Player.Position);

            if (moveClip != null && !_audioSource.isPlaying)
            {
                _audioSource.clip = moveClip;
                _audioSource.loop = true;
                _audioSource.Play();
            }
            _animator.SetFloat("RelativeVelocityX", 0);
            _animator.SetFloat("RelativeVelocityZ", 1);
        }
        else if (ShouldAttack())
        {
            _animator.SetFloat("RelativeVelocityX", 0);
            _animator.SetFloat("RelativeVelocityZ", 0);
            if (!enemyAttack.IsAttacking)
                LookAtTarget(Enemy.Player.transform);

            if (!enemyAttack.IsResting) 
            {
                agent.SetDestination(Enemy.Position);
                StartCoroutine(AttackCoroutine());
            }
            // audioSource.Stop();
        }
        else
        {
            _animator.SetFloat("RelativeVelocityX", 0);
            _animator.SetFloat("RelativeVelocityZ", 0);
            agent.SetDestination(Enemy.Position);
            // audioSource.Stop();
        }
    }

    private bool ShouldChase() {
        if(CanDetect(Enemy.Player) || isChasing) { 
            return !ShouldAttack() && CanChase(Enemy.Player);
        }
        return false;
    }

    private bool ShouldAttack() {
        if (Enemy.DistanceToSq(Enemy.Player) < AttackDistanceSq) { 
            return true;
        }
        return false;    
    }

    private IEnumerator AttackCoroutine()
    {
        if (_attackClips.Length > 0 && !enemyAttack.IsAttacking)
            _audioSource.PlayOneShot(_attackClips[Random.Range(0, _attackClips.Length)], 1f);
        enemyAttack.OnClick();
        yield return new WaitForSeconds(0.2f);
    }

    private bool CanChase(Entity other ) {
        return Enemy.DistanceToSq(other) < ChaseDistanceSq;
    }

    private bool CanDetect(Entity other) {
        return Enemy.DistanceToSq(other) < DetectionDistanceSq; 
            //|| Enemy.CurrentRoom == other.CurrentRoom;
    }

    public override Vector3 CalculateMoveDirection()
    {
        return new Vector3(0, 0, 0);
    }


    public Enemy Enemy { get => this.Entity as Enemy; }
    public Vector3 Velocity { get => Motion.Velocity; set => Motion.Velocity = value; }
}
