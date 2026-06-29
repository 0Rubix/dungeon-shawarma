using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySlot currentSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

        if (currentSlot == null)
        {
            if (InventoryManager.instance.rightClickMenuOpen && !InventoryManager.instance.rightClickMenuSlot.CheckMouseRaycast(eventData.pointerCurrentRaycast.gameObject))
            {
                InventoryManager.instance.rightClickMenuSlot.CloseRightClickMenu();
                InventoryManager.instance.rightClickMenuSlot = null;
                InventoryManager.instance.rightClickMenuOpen = false;
            }
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryManager.instance.rightClickMenuOpen && !InventoryManager.instance.rightClickMenuSlot.CheckMouseRaycast(eventData.pointerCurrentRaycast.gameObject))
            {
                InventoryManager.instance.rightClickMenuSlot.CloseRightClickMenu();
                InventoryManager.instance.rightClickMenuSlot = null;
                InventoryManager.instance.rightClickMenuOpen = false;
                currentSlot.rim.SetActive(true);
                return;
            }

            InventorySlot cursorSlot = InventoryManager.instance.cursorSlot;

            if (cursorSlot.item == null && currentSlot.item == null) return;

            if (currentSlot.item != null && cursorSlot.item == null)
            {
                cursorSlot.Add(currentSlot.item, currentSlot.amount);
                InventoryManager.instance.cursorTookFrom = currentSlot.slot;
                InventoryManager.instance.EmptySlot(currentSlot.slot);
            }
            else if (currentSlot.item == null && cursorSlot.item != null)
            {
                InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount, currentSlot.slot);
                InventoryManager.instance.cursorTookFrom = -1;
                cursorSlot.Clear();
            }
            else if (currentSlot.item != cursorSlot.item || currentSlot.item == cursorSlot.item && currentSlot.amount >= currentSlot.item.maxStack)
            {
                Item tempItem = currentSlot.item;
                int tempAmount = currentSlot.amount;

                currentSlot.Add(cursorSlot.item, cursorSlot.amount);
                InventoryManager.instance.EmptySlot(currentSlot.slot);
                InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount, currentSlot.slot);
                cursorSlot.Add(tempItem, tempAmount);
                InventoryManager.instance.cursorTookFrom = currentSlot.slot;
            }
            else
            {
                int availableAmount = currentSlot.item.maxStack - currentSlot.amount; // How many more items can the slot fit before reaching the stack
                int newAmount = availableAmount > cursorSlot.amount ? cursorSlot.amount : availableAmount;
                int remainingAmount = cursorSlot.amount > availableAmount ? cursorSlot.amount - availableAmount : 0;

                InventoryManager.instance.AddItem(currentSlot.item, newAmount, currentSlot.slot);

                if (newAmount == cursorSlot.amount)
                {
                    InventoryManager.instance.cursorTookFrom = -1;
                    cursorSlot.Clear();
                }
                else
                {
                    cursorSlot.Add(currentSlot.item, remainingAmount);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.instance.rightClickMenuOpen)
            {
                InventoryManager.instance.rightClickMenuSlot.CloseRightClickMenu();
                InventoryManager.instance.rightClickMenuSlot = null;
                InventoryManager.instance.rightClickMenuOpen = false;
            }

            if (currentSlot.item.options == Item.MenuOptions.Nothing) return;

            currentSlot.rightClickMenu.SetActive(true);
            currentSlot.ShowMenuOptions(currentSlot.item.options);
            currentSlot.rim.SetActive(true);
            InventoryManager.instance.rightClickMenuSlot = currentSlot;
            InventoryManager.instance.rightClickMenuOpen = true;
        }
    }
}
