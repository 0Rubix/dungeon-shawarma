using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slots;
    InventorySlot[] inventorySlots;

    void Start()
    {
        inventorySlots = slots.GetComponentsInChildren<InventorySlot>();

        InventoryManager.instance.onInventoryChanged += UpdateUI;
    }

    public void UpdateUI(int s, Item item, int amount)
    {
        InventorySlot slot = inventorySlots[s];

        if (item == null)
        {
            slot.Clear();
        }
        else
        {
            slot.Add(item, amount);
        }
    }
}
