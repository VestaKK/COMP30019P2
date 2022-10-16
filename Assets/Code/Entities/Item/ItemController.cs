using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : EntityController
{
    [SerializeField] float turnSpeed;
    [SerializeField] Entity player;
    [SerializeField] private float distanceToPlayer;

    protected void Awake()
    {
        base.Awake();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() as Entity;
        Entity = this.GetComponent<ItemEntity>() as Entity;
    }

    public override Vector3 CalculateMoveDirection() {
        if (distanceToPlayer <= 3)
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

        if (distanceToPlayer < 1f) 
        {
            ItemEntity itemEntity = this.Entity as ItemEntity;
            PlayerInventory.AddItem((itemEntity).item);
            Destroy(this.gameObject);
        }
    }

    public Item Item { get => (this.Entity as ItemEntity).item ; }
}