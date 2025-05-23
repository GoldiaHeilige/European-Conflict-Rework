using System;
using UnityEngine;

[System.Serializable]
public class ArmorRuntimeItem : InventoryItemRuntime
{
    public ArmorData baseData;
    public int runtimeDurability;

    public ArmorRuntimeItem(ArmorData baseData, int durability, string forcedId = null)
        : base(baseData, 1, durability, forcedId)
    {
        this.baseData = baseData;
        this.runtimeDurability = durability;
        this.durability = durability; // ✅ đồng bộ vào cha
    }
}
