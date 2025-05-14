using System;
using UnityEngine;

[System.Serializable]
public class InventoryItemRuntime
{
    public string runtimeId;
    public InventoryItemData itemData;
    public int quantity;
    public int durability;


    public InventoryItemRuntime(InventoryItemData data, int qty, int? durabilityOverride = null)
    {
        runtimeId = Guid.NewGuid().ToString();
        itemData = data;
        quantity = qty;

        if (data is ArmorData armor)
        {
            durability = durabilityOverride ?? armor.maxDurability;
        }

        Debug.Log($"📦 New InventoryItemRuntime created — ID: {runtimeId} | {itemData?.itemID} | Durability: {durability}");
    }

    public bool IsStackable => itemData != null && itemData.stackable;
}