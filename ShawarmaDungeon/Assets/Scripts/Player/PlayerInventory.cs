using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    PlayerInput input;

    private void Awake()
    {
        input = new PlayerInput();
        input.Interact.Enable();

        input.Interact.Inventory.performed += Inventory;
        input.Interact.Escape.performed += EscapeCloseInventory;
        input.Interact.Test.performed += PrintInventory;
        input.Interact.Hotbar.started += OnHotbarPress;
    }

    private void Start()
    {
        for (int i = 0; i < InventoryManager.instance.hotbarSlots.Length; i++)
        {
            InventoryManager.instance.hotbarSlots[i].SetHotkey(input.Interact.Hotbar.GetBindingDisplayString(i));
        }
    }

    private void OnEnable()
    {
        input.Interact.Enable();
    }

    private void OnDisable()
    {
        input.Interact.Disable();
    }

    [SerializeField] private GameObject inventoryUI;

    public void Inventory(InputAction.CallbackContext ctx)
    {
        if (InventoryManager.instance.isOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    void CloseInventory()
    {
        inventoryUI.SetActive(false);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAim>().enabled = true;
        InventoryManager.instance.isOpen = false;

        InventoryManager.instance.onInventoryToggle.Invoke();
    }

    void EscapeCloseInventory(InputAction.CallbackContext ctx)
    {
        if (InventoryManager.instance.isOpen)
            CloseInventory();
    }

    void OpenInventory()
    {
        inventoryUI.SetActive(true);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAim>().enabled = false;
        InventoryManager.instance.isOpen = true;
    }

    void PrintInventory(InputAction.CallbackContext context)
    {
        foreach (var slot in InventoryManager.instance.inventory)
        {
            Debug.Log(slot.Key == null ? "Null" : slot.Key.name + " " + slot.Value);
        }
    }

    void OnHotbarPress(InputAction.CallbackContext context)
    {
        int bindingIndex = context.action.GetBindingIndexForControl(context.control);
        InventoryManager.instance.hotbarSlots[InventoryManager.instance.hotbarSlot].rim.SetActive(false);
        InventoryManager.instance.hotbarSlot = bindingIndex;
        InventoryManager.instance.hotbarSlots[bindingIndex].rim.SetActive(true);

        if (InventoryManager.instance.hotbarSlots[bindingIndex].item == null)
            PlayerManager.instance.SwitchHandGFX(null);
        else
            PlayerManager.instance.SwitchHandGFX(InventoryManager.instance.hotbarSlots[bindingIndex].item.icon);
    }
}
