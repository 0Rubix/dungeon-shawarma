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

    public KeyValuePair<Item, int>[] inventory;
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
        inventory = new KeyValuePair<Item, int>[hotbarSpace + inventorySpace]; // The inventory will be the hotbar slots + inventory slots, starting with the hotbar
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
            KeyValuePair<Item, int> slot = inventory[i];

            if (slot.Key != item && slot.Value > 0) continue; // Current slot is not null and is not the same as the added item
            if (slot.Value >= item.maxStack) continue; // Current slot amount should be less than the added item max stack. we already made sure the item is the same or null

            KeyValuePair<Item, int> newSlot = new KeyValuePair<Item, int>();

            if (slot.Key == null)
            {
                newSlot = new KeyValuePair<Item, int>(item, amount);
                inventory[i] = newSlot;
            }
            else
            {
                int availableAmount = item.maxStack - slot.Value; // How many more items can the slot fit before reaching the stack
                int newAmount = availableAmount > amount ? amount : availableAmount;
                int continueAmount = amount > availableAmount ? amount - availableAmount : 0; // The amount to use in the recursion call

                newSlot = new KeyValuePair<Item, int>(item, slot.Value + newAmount);
                inventory[i] = newSlot;

                AddItem(item, continueAmount);
            }

            onInventoryChanged.Invoke(i, newSlot.Key, newSlot.Value);

            if (i == hotbarSlot)
                PlayerManager.instance.SwitchHandGFX(newSlot.Key.icon);

            return true;
        }

        return false;
    }

    public bool AddItem(Item item, int amount, int slot)
    {
        if (amount == 0) return false;

        // Get current slot
        KeyValuePair<Item, int> currentSlot = inventory[slot];

        if (currentSlot.Key != item && currentSlot.Value > 0) return true; ; // Current slot is not null and is not the same as the added item
        if (currentSlot.Value >= item.maxStack) return true; ; // Current slot amount should be less than the added item max stack. we already made sure the item is the same or null

        KeyValuePair<Item, int> newSlot = new KeyValuePair<Item, int>();

        if (currentSlot.Key == null)
        {
            newSlot = new KeyValuePair<Item, int>(item, amount);
            inventory[slot] = newSlot;
        }
        else
        {
            int availableAmount = item.maxStack - currentSlot.Value; // How many more items can the slot fit before reaching the stack
            int newAmount = availableAmount > amount ? amount : availableAmount;
            int continueAmount = amount > availableAmount ? amount - availableAmount : 0; // The amount to use in the recursion call

            newSlot = new KeyValuePair<Item, int>(item, currentSlot.Value + newAmount);
            inventory[slot] = newSlot;

            AddItem(item, continueAmount);
        }

        onInventoryChanged.Invoke(slot, newSlot.Key, newSlot.Value);

        return true;
    }

    public void EmptySlot(int slot)
    {
        inventory[slot] = new KeyValuePair<Item, int>(null, 0);
        onInventoryChanged.Invoke(slot, null, 0);

        if (slot == hotbarSlot)
            PlayerManager.instance.SwitchHandGFX(null);
    }

    public void RemoveFromSlot(int slot, int amount)
    {
        if (amount == 0) return;

        // Get current slot
        KeyValuePair<Item, int> currentSlot = inventory[slot];

        if (amount > currentSlot.Value) return;

        if (currentSlot.Key == null) return;

        KeyValuePair<Item, int> newSlot = new KeyValuePair<Item, int>();
        int newAmount = currentSlot.Value - amount;

        if (newAmount == 0)
        {
            EmptySlot(slot);
        }
        else
        {
            newSlot = new KeyValuePair<Item, int>(currentSlot.Key, newAmount);
            inventory[slot] = newSlot;
            onInventoryChanged.Invoke(slot, newSlot.Key, newSlot.Value);
        }
    }
}
