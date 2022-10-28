using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public delegate void PlayerInventoryUpdate(ItemSlot itemSlot);
    public static event PlayerInventoryUpdate OnInventoryUpdate;

    public delegate void PlayerInventoryUpdateBullets();
    public static event PlayerInventoryUpdateBullets OnInventoryUpdateBullets;

    private Slot<Weapon> _mainWeaponSlot;
    private Slot<Weapon> _offHandSlot;

    [SerializeField] int _numBullets;

    [SerializeField] private List<ItemSlot> itemSlots;
    private Dictionary<Item, ItemSlot> itemDictionary;

    private static PlayerInventory _instance;

    void Awake() {

        if (_instance == null)
        {
            _instance = this;

            _instance._mainWeaponSlot = new Slot<Weapon>(this);
            _instance._offHandSlot = new Slot<Weapon>(this);
            _instance.itemSlots = new List<ItemSlot>();
            _instance.itemDictionary = new Dictionary<Item, ItemSlot>();
            _instance._numBullets = 10;
            OnInventoryUpdateBullets.Invoke();
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
            Debug.Log(itemSlot.item.name);
            _instance.itemDictionary.Add(newItem, itemSlot);
            _instance.itemSlots.Add(itemSlot);
            OnInventoryUpdate.Invoke(itemSlot);
        }

        

        if (newItem.id == 2)
        {
            AddBullet(2);
        }

    }

    public static List<ItemSlot> GetItems() 
    {
        return _instance.itemSlots;
    }

    public static bool HasBullets()
    {
        return NumBullets > 0;
    }

    public static void FireBullet() 
    {
        NumBullets--;
        OnInventoryUpdateBullets.Invoke();
    }

    public static void AddBullet()
    {
        NumBullets++;
        OnInventoryUpdateBullets.Invoke();
    }

    public static void AddBullet(int amount)
    {
        NumBullets += amount;
        OnInventoryUpdateBullets.Invoke();
    }


    public static int NumBullets { get => _instance._numBullets; set => _instance._numBullets = value; }
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