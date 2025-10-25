using UnityEngine;

public class ItemPrefab : Interactable
{
    public string itemName;
    [SerializeField] private ItemDatabase db;

    SpriteRenderer spriteRenderer;
    BoxCollider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        Vector2 colSize;
        colSize.x = spriteRenderer.bounds.size.x;
        colSize.y = spriteRenderer.bounds.size.y / 2;
        col.size = colSize;

        Vector2 colOffset;
        colOffset.x = 0;
        colOffset.y = spriteRenderer.bounds.center.y / 2;
        col.offset = colOffset;
    }

    public override void Use()
    {
        InventoryManager.Instance.AddItem(itemName);
        base.Use();
    }
}
