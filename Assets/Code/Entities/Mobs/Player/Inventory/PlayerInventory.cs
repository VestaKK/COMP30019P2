using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Image _uiSlot;
    private Slot _mainWeaponSlot;
    private Slot _offHandSlot;

    void Awake() {
        _mainWeaponSlot = new Slot(this);
        _offHandSlot = new Slot(this);
    }
}