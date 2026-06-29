using System.Collections.Generic;
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
}
