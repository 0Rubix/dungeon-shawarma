using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public int slot = 0;
    public Item item;
    public int amount;

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI hotkeyText;
    public GameObject rim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryManager.instance.onInventoryChanged += UpdateSlot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlot(int slot, Item newItem, int newAmount)
    {
        if (slot != this.slot) return;

        if (newItem == null)
        {
            item = newItem;
            amount = newAmount;
            icon.enabled = false;
            icon.sprite = null;
            amountText.text = "";
        }
        else
        {
            item = newItem;
            amount = newAmount;
            icon.enabled = true;
            icon.sprite = item.inventoryIcon;
            amountText.text = amount <= 1 ? "" : amount.ToString();
        }
    }

    public void SetHotkey(string hotkey)
    {
        hotkeyText.text = hotkey;
    }
}
