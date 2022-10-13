using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    private PlayerInventory _inventory;

    private Item _containedItem;

    public Slot(PlayerInventory i) {
        this._inventory = i;
    }
}