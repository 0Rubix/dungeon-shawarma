using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform hotbarSlots;
    [SerializeField] private Transform inventorySlots;
    List<InventorySlot> slots = new List<InventorySlot>();

    void Start()
    {
        int slotIndex = 0;
        foreach (var slot in hotbarSlots.GetComponentsInChildren<InventorySlot>())
        {
            slot.slot = slotIndex;
            slots.Add(slot);
            slotIndex++;
        }

        foreach (var slot in inventorySlots.GetComponentsInChildren<InventorySlot>())
        {
            slot.slot = slotIndex;
            slots.Add(slot);
            slotIndex++;
        }

        InventoryManager.instance.onInventoryChanged += UpdateUI;

    }

    public void UpdateUI(int s, Item item, int amount)
    {
        InventorySlot slot = slots[s];

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
