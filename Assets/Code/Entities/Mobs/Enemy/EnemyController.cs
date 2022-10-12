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

    public override Vector3 CalculateMoveDirection()
    {
        return new Vector3(0, 0, 0);
    }

    public Vector3 Velocity { get => Motion.Velocity; set => Motion.Velocity = value; }
}
