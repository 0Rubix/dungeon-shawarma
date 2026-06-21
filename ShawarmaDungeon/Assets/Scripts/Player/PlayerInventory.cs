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
}
