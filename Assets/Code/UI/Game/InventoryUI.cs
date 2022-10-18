using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UIPanel {

    [SerializeField] GameObject relicView;
    [SerializeField] GameObject slotPrefab;
    private Dictionary<ItemSlot, GameObject> itemToSlot = new Dictionary<ItemSlot, GameObject>();

    public override void Initialise()
    {
        PlayerInventory.OnInventoryUpdate += UpdateInventory;
    }

    private void UpdateInventory(ItemSlot itemSlot) 
    {
        if (itemToSlot.TryGetValue(itemSlot, out GameObject itemView))
        {
            itemView.GetComponentInChildren<Text>().text = itemSlot.count.ToString();
        }
        else
        {
            itemView = Instantiate(slotPrefab, relicView.transform);
            itemView.GetComponentInChildren<Text>().text = itemSlot.count.ToString();
            itemView.GetComponentsInChildren<Image>()[1].GetComponent<Image>().sprite = itemSlot.item.icon;
            itemToSlot.Add(itemSlot, itemView);
        }
    }
}
