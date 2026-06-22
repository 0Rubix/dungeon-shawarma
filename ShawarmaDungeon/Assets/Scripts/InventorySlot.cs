using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    public int amount = 0;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

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
}
