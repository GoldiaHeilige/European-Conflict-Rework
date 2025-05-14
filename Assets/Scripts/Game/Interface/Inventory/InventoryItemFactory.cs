using UnityEngine;

public static class InventoryItemFactory
{
    public static InventoryItemRuntime Create(InventoryItemData itemData, int quantity = 1)
    {
        int? durability = null;

        if (itemData is ArmorData armor)
        {
            durability = armor.maxDurability;
        }

        var newItem = new InventoryItemRuntime(itemData, quantity, durability);
        Debug.Log($"📦 Create → ID: {newItem.runtimeId} | {itemData?.itemID} | Durability: {newItem.durability}");

        return newItem;
    }

    public static InventoryItemRuntime Clone(InventoryItemRuntime source)
    {
        if (source == null) return null;

        var clone = new InventoryItemRuntime(source.itemData, source.quantity, source.durability);
        Debug.Log($"[🌀 Clone] From: {source.runtimeId} → {clone.runtimeId} | itemData: {source.itemData?.itemID ?? "NULL"}");

        return clone;
    }
}
