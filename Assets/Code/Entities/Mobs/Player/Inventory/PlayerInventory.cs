using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public delegate void PlayerInventoryUpdate(ItemSlot itemSlot);
    public static event PlayerInventoryUpdate OnInventoryUpdate;


    private Slot<Weapon> _mainWeaponSlot;
    private Slot<Weapon> _offHandSlot;

    [SerializeField] private List<ItemSlot> itemSlots = new List<ItemSlot>();
    private Dictionary<Item, ItemSlot> itemDictionary = new Dictionary<Item, ItemSlot>();

    private static PlayerInventory _instance;

    void Awake() {

        if (_instance == null)
        {
            _instance = this;

            _instance._mainWeaponSlot = new Slot<Weapon>(this);
            _instance._offHandSlot = new Slot<Weapon>(this);

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }   
    }

    public static void AddItem(Item newItem) 
    {
        if (newItem == null) return;

        if (_instance.itemDictionary.TryGetValue(newItem, out ItemSlot itemSlot))
        {
            itemSlot.count++;
            OnInventoryUpdate.Invoke(itemSlot);
        }
        else 
        {
            itemSlot = new ItemSlot(newItem);
            _instance.itemDictionary.Add(newItem, itemSlot);
            _instance.itemSlots.Add(itemSlot);
            OnInventoryUpdate.Invoke(itemSlot);
        }

        
    }

    public static List<ItemSlot> GetItems() 
    {
        return _instance.itemSlots;
    }
}


[System.Serializable]
public class ItemSlot
{

    public ItemSlot(Item item)
    {
        this.item = item;
        count++;
    }

    public Item item = null;
    public int count = 0;
}