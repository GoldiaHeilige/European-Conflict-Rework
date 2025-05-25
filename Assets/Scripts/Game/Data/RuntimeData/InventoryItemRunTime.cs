using System;
using UnityEngine;

[System.Serializable]
public class InventoryItemRuntime
{
    public string runtimeId;
    public InventoryItemData itemData;
    public int quantity;
    public int durability;

    public float TotalWeight => itemData != null ? itemData.weightPerUnit * quantity : 0f;

    public InventoryItemRuntime(InventoryItemData data, int qty, int? durabilityOverride = null, string forcedId = null)
    {
        runtimeId = forcedId ?? Guid.NewGuid().ToString();
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