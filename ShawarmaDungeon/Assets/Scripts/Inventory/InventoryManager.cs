using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] int space = 15;
    [SerializeField] private KeyValuePair<Item, int>[] inventory;

    [SerializeField] private ItemDatabase db;

    private void Awake()
    {
        Instance = this;

        inventory = new KeyValuePair<Item, int>[space];
    }


    public bool AddItem(string itemName)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            KeyValuePair<Item, int> slot = inventory[i];

            if (slot.Value > 0 && slot.Key != db.GetItem(itemName)) continue;

            if (slot.Key != null && slot.Value >= slot.Key.maxStack) continue;

            KeyValuePair<Item, int> newSlot = new KeyValuePair<Item, int>(db.GetItem(itemName), slot.Value + 1);
            inventory[i] = newSlot;

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
