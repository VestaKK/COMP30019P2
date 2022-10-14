using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Image _uiSlot;
    private Slot<Weapon> _mainWeaponSlot;
    private Slot<Weapon> _offHandSlot; 

    void Awake() {
        _mainWeaponSlot = new Slot<Weapon>(this);
        _offHandSlot = new Slot<Weapon>(this);
    }
}