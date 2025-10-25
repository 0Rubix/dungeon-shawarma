using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "New Database", menuName = "Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializedDictionary("Item Name", "Item")]
    public SerializedDictionary<string, Item> allItems;

    public Item GetItem(string itemName)
    {
        bool itemExists = allItems.TryGetValue(itemName, out var item);
        return item;
    }
}
