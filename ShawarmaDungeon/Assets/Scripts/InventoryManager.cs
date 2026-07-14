using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    public delegate void InventoryChanged(int slot, Item item, int amount);
    public InventoryChanged onInventoryChanged;

    public delegate void InventoryToggled();
    public InventoryToggled onInventoryToggle;

    private void Awake()
    {
        instance = this;
    }

    public GameObject itemPrefab;

    public bool isOpen = false;

    public InventoryItem[] inventory;
    public int hotbarSpace = 5;
    public int inventorySpace = 15;

    public int hotbarSlot = 0;
    public HotbarSlot[] hotbarSlots;

    public InventorySlot cursorSlot;
    public int cursorTookFrom = -1;

    public InventorySlot rightClickMenuSlot;
    public bool rightClickMenuOpen = false;

    void Start()
    {
        inventory = new InventoryItem[hotbarSpace + inventorySpace]; // The inventory will be the hotbar slots + inventory slots, starting with the hotbar
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventoryItem();
        }
    }

    void Update()
    {
        cursorSlot.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
    }

    public bool AddItem(Item item, int amount)
    {
        if (amount == 0) return false;

        // Find a suitable slot
        for (int i = 0; i < inventory.Length; i++)
        {
            // Get current slot
            InventoryItem slot = inventory[i];

            if (slot.item != item && slot.amount > 0) continue; // Current slot is not null and is not the same as the added item
            if (slot.amount >= item.maxStack) continue; // Current slot amount should be less than the added item max stack. we already made sure the item is the same or null

            InventoryItem newSlot = new InventoryItem();

            if (slot.item == null)
            {
                newSlot = new InventoryItem(item, amount);
                inventory[i] = newSlot;
            }
            else
            {
                int availableAmount = item.maxStack - slot.amount; // How many more items can the slot fit before reaching the stack
                int newAmount = availableAmount > amount ? amount : availableAmount;
                int continueAmount = amount > availableAmount ? amount - availableAmount : 0; // The amount to use in the recursion call

                newSlot = new InventoryItem(item, slot.amount + newAmount);
                inventory[i] = newSlot;

                AddItem(item, continueAmount);
            }

            onInventoryChanged.Invoke(i, newSlot.item, newSlot.amount);

            return true;
        }

        return false;
    }

    public bool AddItem(Item item, int amount, int slot)
    {
        if (amount == 0) return false;

        // Get current slot
        InventoryItem currentSlot = inventory[slot];

        if (currentSlot.item != item && currentSlot.amount > 0) return true; ; // Current slot is not null and is not the same as the added item
        if (currentSlot.amount >= item.maxStack) return true; ; // Current slot amount should be less than the added item max stack. we already made sure the item is the same or null

        InventoryItem newSlot = new InventoryItem();

        if (currentSlot.item == null)
        {
            newSlot = new InventoryItem(item, amount);
            inventory[slot] = newSlot;
        }
        else
        {
            int availableAmount = item.maxStack - currentSlot.amount; // How many more items can the slot fit before reaching the stack
            int newAmount = availableAmount > amount ? amount : availableAmount;
            int continueAmount = amount > availableAmount ? amount - availableAmount : 0; // The amount to use in the recursion call

            newSlot = new InventoryItem(item, currentSlot.amount + newAmount);
            inventory[slot] = newSlot;

            AddItem(item, continueAmount);
        }

        onInventoryChanged.Invoke(slot, newSlot.item, newSlot.amount);

        return true;
    }

    public void EmptySlot(int slot)
    {
        inventory[slot] = new InventoryItem();
        onInventoryChanged.Invoke(slot, null, 0);
    }

    public void RemoveFromSlot(int slot, int amount)
    {
        if (amount == 0) return;

        // Get current slot
        InventoryItem currentSlot = inventory[slot];

        if (amount > currentSlot.amount) return;

        if (currentSlot.item == null) return;

        InventoryItem newSlot = new InventoryItem();
        int newAmount = currentSlot.amount - amount;

        if (newAmount == 0)
        {
            EmptySlot(slot);
        }
        else
        {
            newSlot = new InventoryItem(currentSlot.item, newAmount);
            inventory[slot] = newSlot;
            onInventoryChanged.Invoke(slot, newSlot.item, newSlot.amount);
        }
    }
}
