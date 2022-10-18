using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MobController
{
    private void Awake()
    {
        base.Awake();
        Entity = this.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        EntityMove();
    }

    public void EntityMove() {
        Debug.Log(Enemy.Player.Position);
        if(ShouldChase()) {
                agent.SetDestination(Enemy.Player.Position);
        } else agent.SetDestination(Enemy.Position);
    }

    private bool ShouldChase() { 
        if(Enemy.IsInSameRoom(Enemy.Player)) {
            return Enemy.DistanceTo(Enemy.Player) > Enemy.Player.Radius * 2.5;
        }

        return false;
    }

    public override Vector3 CalculateMoveDirection()
    {
        return new Vector3(0, 0, 0);
    }


    public Enemy Enemy { get => this.Entity as Enemy; }
    public Vector3 Velocity { get => Motion.Velocity; set => Motion.Velocity = value; }
}
