using UnityEngine;

public class ItemPrefab : Interactable
{
    public Item item;
    public int amount = 1;

    SpriteRenderer spriteRenderer;
    BoxCollider2D col;

    private void Awake()
    {
        if (item != null)
            Setup(item);
    }

    public void Setup(Item item)
    {
        this.item = item;

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
        bool added = InventoryManager.instance.AddItem(item, amount);

        if (added)
            base.Use();
    }
}
