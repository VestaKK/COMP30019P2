using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : EntityController
{

    public override Vector3 CalculateMoveDirection() {
        return Vector3.zero;
    }

    public Item Item { get => this.Entity as Item; }
}