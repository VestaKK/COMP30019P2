using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : EntityController
{
    [SerializeField] private float distanceToPlayer;

    float turnSpeed = 100f;
    private float noPickupTime = 0.5f;

    protected void Awake()
    {
        base.Awake();
        Entity = this.GetComponent<ItemEntity>() as Entity;
    }

    public override Vector3 CalculateMoveDirection() {
        if (distanceToPlayer <= 3 && noPickupTime <= 0)
        {
            return (Player.transform.position - Entity.transform.position).normalized;
        }

        return Vector3.zero;
    }

    protected void Update() {
        if (GameManager.isPaused && Player != null) return;
        base.Update();
        distanceToPlayer = Entity.DistanceTo(Player);
        transform.rotation *= Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
        if (Player.isDead) return;

        Motion.UpdateVelocity();
        EntityMove();

        if (distanceToPlayer < 0.3f && noPickupTime <= 0) 
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
    public Player Player { get => GameManager.CurrentPlayer; }
}