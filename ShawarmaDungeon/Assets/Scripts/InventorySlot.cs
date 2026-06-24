using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int slot = 0;
    public Item item;
    public int amount = 0;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject rim;

    private void Start()
    {
        InventoryManager.instance.onInventoryToggle += ResetUI;
    }

    public void Add(Item newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
        icon.sprite = item.inventoryIcon;
        icon.enabled = true;
        amountText.text = amount <= 1 ? "" : amount.ToString();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != gameObject) return;

        InventorySlot cursorSlot = InventoryManager.instance.cursorSlot;

        if (cursorSlot.item == null && item == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null && cursorSlot.item == null)
            {
                cursorSlot.Add(item, amount);
                InventoryManager.instance.cursorTookFrom = slot;
                InventoryManager.instance.EmptySlot(slot);
            }
            else if (item == null && cursorSlot.item != null)
            {
                InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount, slot);
                InventoryManager.instance.cursorTookFrom = -1;
                cursorSlot.Clear();
            }
            else if (item != cursorSlot.item || item == cursorSlot.item && amount >= item.maxStack)
            {
                Item tempItem = item;
                int tempAmount = amount;

                Add(cursorSlot.item, cursorSlot.amount);
                InventoryManager.instance.EmptySlot(slot);
                InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount, slot);
                cursorSlot.Add(tempItem, tempAmount);
                InventoryManager.instance.cursorTookFrom = slot;
            }
            else
            {
                int availableAmount = item.maxStack - amount; // How many more items can the slot fit before reaching the stack
                int newAmount = availableAmount > cursorSlot.amount ? cursorSlot.amount : availableAmount;
                int remainingAmount = cursorSlot.amount > availableAmount ? cursorSlot.amount - availableAmount : 0;

                InventoryManager.instance.AddItem(item, newAmount, slot);

                if (newAmount == cursorSlot.amount)
                {
                    InventoryManager.instance.cursorTookFrom = -1;
                    cursorSlot.Clear();
                }
                else
                {
                    cursorSlot.Add(item, remainingAmount);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rim.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rim.SetActive(false);
    }

    public void ResetUI()
    {
        rim.SetActive(false);

        InventorySlot cursorSlot = InventoryManager.instance.cursorSlot;
        int slot = InventoryManager.instance.cursorTookFrom;

        if (slot != this.slot) return;

        if (item != null)
            InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount);
        else
            InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount, slot);

        cursorSlot.Clear();
    }
}
