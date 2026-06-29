using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public new string name = "New Item";
    public Sprite icon;
    public Sprite inventoryIcon;
    public int maxStack;

    [Flags]
    public enum MenuOptions
    {
        Nothing = 0,
        Use = 1 << 0,
        Scrap = 1 << 1,
        QuickCraft = 1 << 2,
        Drop = 1 << 3,
        DropAll = 1 << 4,
        Everything = ~0
    };
    public MenuOptions options;

    public virtual void Use()
    {
        if (!options.HasFlag(MenuOptions.Use)) return;
        Debug.Log("Using " + name);
    }

    public virtual void Scrap()
    {
        if (!options.HasFlag(MenuOptions.Scrap)) return;
        Debug.Log("Scrapping " + name);
    }

    public virtual void QuickCraft()
    {
        if (!options.HasFlag(MenuOptions.QuickCraft)) return;
        Debug.Log("Crafting " + name + "'s Recipe");
    }
}
