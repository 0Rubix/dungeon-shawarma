using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    public delegate void InventoryChanged(int slot, Item item, int amount);
    public InventoryChanged onInventoryChanged;

    private void Awake()
    {
        instance = this;
    }

    public bool isOpen = false;

    public KeyValuePair<Item, int>[] inventory;
    public int space = 15;

    void Start()
    {
        inventory = new KeyValuePair<Item, int>[space];
    }

    void Update()
    {
        
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++) // Loop through all the slots
        {
            KeyValuePair<Item, int> slot = inventory[i]; // get the current slot

            // idk what happens here but if it works i aint touching it
            if (slot.Value > 0 && slot.Key != item) continue;
            if (slot.Key != null && slot.Value >= slot.Key.maxStack) continue;

            KeyValuePair<Item, int> newSlot = new KeyValuePair<Item, int>(item, slot.Value + 1);
            inventory[i] = newSlot;

            onInventoryChanged.Invoke(i, newSlot.Key, newSlot.Value);

            foreach (KeyValuePair<Item, int> s in inventory)
            {
                Debug.Log((s.Key == null ? "Null" : s.Key) + ": " + s.Value);
            }


            return true;
        }

        foreach (KeyValuePair<Item, int> slot in inventory)
        {
            Debug.Log(slot.Key + ": " + slot.Value);
        }

        return false;
    }
}
