using UnityEngine;

public class ItemPrefab : Interactable
{
    public Item item;

    SpriteRenderer spriteRenderer;
    BoxCollider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        spriteRenderer.sprite = item.icon;

        Vector2 colSize;
        colSize.x = spriteRenderer.bounds.size.x;
        colSize.y = spriteRenderer.bounds.size.y / 3;
        col.size = colSize;

        //Vector2 colOffset;
        //colOffset.x = 0;
        //colOffset.y = spriteRenderer.bounds.center.y / 3;
        //col.offset = colOffset;
    }

    public override void Use()
    {
        bool added = InventoryManager.instance.AddItem(item);

        if (added)
            base.Use();
    }
}
