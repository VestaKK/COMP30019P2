using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryUI : UIPanel {

    [SerializeField] Transform relicView;
    [SerializeField] GameObject slotPrefab;
    private Dictionary<ItemSlot, GameObject> itemToSlot;

    [SerializeField] TMP_Text numBulletsText;

    public override void Initialise()
    {
        itemToSlot = new Dictionary<ItemSlot, GameObject>();
        PlayerInventory.OnInventoryUpdate += UpdateInventory;
        PlayerInventory.OnInventoryUpdateBullets += UpdateBullets;
    }

    private void UpdateInventory(ItemSlot itemSlot)
    { 
        if (itemToSlot.TryGetValue(itemSlot, out GameObject itemView))
        {
            itemView.GetComponentInChildren<Text>().text = itemSlot.count.ToString();
        }
        else
        {
            itemView = Instantiate(slotPrefab, relicView);
            itemView.GetComponentInChildren<Text>().text = itemSlot.count.ToString();
            itemView.GetComponentsInChildren<Image>()[1].GetComponent<Image>().sprite = itemSlot.item.icon;
            itemToSlot.Add(itemSlot, itemView);
        }
    }

    private void UpdateBullets()
    {
        Debug.Log("Bingus");
        numBulletsText.text = PlayerInventory.NumBullets.ToString();
    }
}
