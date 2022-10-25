using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MobController
{
    [SerializeField] float DetectionDistanceSq = 0.0f;
    [SerializeField] float AttackDistanceSq = 0.0f;
    [SerializeField] float ChaseDistanceSq = 0.0f;
    [SerializeField] AttackController enemyAttack;
    [SerializeField] bool isChasing;

    private void Awake()
    {
        base.Awake();
        Entity = this.GetComponent<Enemy>();
        agent.speed = Entity.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (GameManager.isPaused) return;
        EntityMove();
        // Check Attack
    }

    public void EntityMove() {
        if ((isChasing = ShouldChase()) && !enemyAttack.IsAttacking)
        {
            LookAtTarget(Enemy.Player.transform);
            agent.SetDestination(Enemy.Player.Position);
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

        }
        else
        {
            _animator.SetFloat("RelativeVelocityX", 0);
            _animator.SetFloat("RelativeVelocityZ", 0);
            agent.SetDestination(Enemy.Position);
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
        enemyAttack.OnClick();
        yield return new WaitForSeconds(0.2f);
    }

    private bool CanChase(Entity other ) {
        return Enemy.DistanceToSq(other) < ChaseDistanceSq;
    }

    private bool CanDetect(Entity other) {
        return Enemy.DistanceToSq(other) < DetectionDistanceSq;
    }

    public override Vector3 CalculateMoveDirection()
    {
        return new Vector3(0, 0, 0);
    }


    public Enemy Enemy { get => this.Entity as Enemy; }
    public Vector3 Velocity { get => Motion.Velocity; set => Motion.Velocity = value; }
}
