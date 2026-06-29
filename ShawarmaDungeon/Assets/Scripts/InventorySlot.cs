using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slot = 0;
    public Item item;
    public int amount = 0;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    public GameObject rim;
    public GameObject rightClickMenu;
    [SerializeField] private GameObject[] rightClickButtons;

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

    public void Use()
    {
        item.Use();
        InventoryManager.instance.RemoveFromSlot(slot, 1);

        if (amount == 0)
            ResetUI();
    }

    public void Scrap()
    {
        item.Scrap();
        InventoryManager.instance.RemoveFromSlot(slot, 1);

        if (amount == 0)
            ResetUI();
    }

    public void QuickCraft()
    {
        item.QuickCraft();
        InventoryManager.instance.RemoveFromSlot(slot, 1);

        if (amount == 0)
            ResetUI();
    }

    public void Drop()
    {
        ItemPrefab itemPrefab = Instantiate(InventoryManager.instance.itemPrefab, GameManager.instance.player.transform.position, Quaternion.identity).GetComponent<ItemPrefab>();
        itemPrefab.Setup(item);
        InventoryManager.instance.RemoveFromSlot(slot, 1);

        if (amount == 0)
            ResetUI();
    }

    public void DropAll()
    {
        for (int i = 0; i < amount; i++)
        {
            ItemPrefab itemPrefab = Instantiate(InventoryManager.instance.itemPrefab, GameManager.instance.player.transform.position, Quaternion.identity).GetComponent<ItemPrefab>();
            itemPrefab.Setup(item);
        }
        InventoryManager.instance.RemoveFromSlot(slot, amount);
        ResetUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!InventoryManager.instance.rightClickMenuOpen)
            rim.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!InventoryManager.instance.rightClickMenuOpen)
            rim.SetActive(false);
    }

    public void ResetUI()
    {
        rim.SetActive(false);
        if (InventoryManager.instance.rightClickMenuSlot != null)
        {
            InventoryManager.instance.rightClickMenuSlot.CloseRightClickMenu();
            InventoryManager.instance.rightClickMenuSlot = null;
            InventoryManager.instance.rightClickMenuOpen = false;
        }

        InventorySlot cursorSlot = InventoryManager.instance.cursorSlot;
        int slot = InventoryManager.instance.cursorTookFrom;

        if (slot != this.slot) return;

        if (item != null)
            InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount);
        else
            InventoryManager.instance.AddItem(cursorSlot.item, cursorSlot.amount, slot);

        cursorSlot.Clear();

    }

    public void ShowMenuOptions(Item.MenuOptions options)
    {
        int i = 0;
        foreach (Item.MenuOptions option in Enum.GetValues(typeof(Item.MenuOptions)))
        {
            if (option == Item.MenuOptions.Nothing || option == Item.MenuOptions.Everything)
                continue;

            rightClickButtons[i].SetActive(options.HasFlag(option));
            i++;
        }
    }

    public void ClearOptions()
    {
        for (int i = 0; i <  rightClickButtons.Length; i++) rightClickButtons[i].SetActive(false);
    }

    public void CloseRightClickMenu()
    {
        rightClickMenu.SetActive(false);
        rim.SetActive(false);
    }

    public bool CheckMouseRaycast(GameObject g)
    {
        foreach (GameObject button in rightClickButtons)
        {
            if (g == button) return true;
        }

        return false;
    }
}
