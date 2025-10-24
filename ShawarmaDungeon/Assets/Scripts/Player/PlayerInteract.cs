using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform interactPos;
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask interactLayers;

    PlayerInput input;

    private void Awake()
    {
        input = new PlayerInput();
        input.Interact.Enable();

        input.Interact.Interact.performed += Interact;
    }

    void Interact(InputAction.CallbackContext context)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(interactPos.position, interactRadius, interactLayers);
        foreach (Collider2D col in cols)
        {
            Interactable interactable = col.GetComponent<Interactable>();

            if (interactable == null) continue;

            interactable.Use();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactPos.position, interactRadius);
    }
}
