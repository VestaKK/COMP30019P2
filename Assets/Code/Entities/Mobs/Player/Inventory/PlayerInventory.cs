using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // TODO: These can be redefined as necessary
    private Slot _mainWeaponSlot;
    private Slot _offHandSlot;

    void Awake() {
        _mainWeaponSlot = new Slot(this);
        _offHandSlot = new Slot(this);
    }
}