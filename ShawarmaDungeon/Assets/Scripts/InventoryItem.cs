[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int amount;

    public InventoryItem()
    {
        item = null;
        amount = 0;
    }

    public InventoryItem(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
