using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : EntityController
{
    [SerializeField] Entity player;
    [SerializeField] private float distanceToPlayer;

    float turnSpeed = 100f;
    private float noPickupTime = 1f;

    protected void Awake()
    {
        base.Awake();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() as Entity;
        Entity = this.GetComponent<ItemEntity>() as Entity;
    }

    public override Vector3 CalculateMoveDirection() {
        if (distanceToPlayer <= 3 && noPickupTime <= 0)
        {
            return (player.transform.position - Entity.transform.position).normalized;
        }

        return Vector3.zero;
    }

    protected void Update() { 
        base.Update();
        distanceToPlayer = Entity.DistanceTo(player);
        Motion.UpdateVelocity();
        transform.rotation *= Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
        EntityMove();

        if (distanceToPlayer < 1f && noPickupTime <= 0) 
        {
            ItemEntity itemEntity = this.Entity as ItemEntity;
            PlayerInventory.AddItem((itemEntity).item);

            DestroyImmediate(this.gameObject);
        }

        if (noPickupTime <= 0)
            noPickupTime = 0;
        else
            noPickupTime -= Time.deltaTime;
    }

    public Item Item { get => (this.Entity as ItemEntity).item ; }
}