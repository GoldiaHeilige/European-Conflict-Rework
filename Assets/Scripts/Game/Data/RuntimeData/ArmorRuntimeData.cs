
using UnityEngine;

public class ArmorRuntime
{
    public ArmorData armorData { get; private set; }
    public InventoryItemRuntime sourceItem { get; private set; }
    public EquippedArmorManager ownerManager { get; private set; }

    public int durability;

    public ArmorSlot Slot => armorData.armorSlot;
    public int currentDurability => durability;

    public static event System.Action OnAnyDurabilityChanged;

    public ArmorRuntime(ArmorData data, EquippedArmorManager manager, InventoryItemRuntime source)
    {
        armorData = data;
        sourceItem = source;
        ownerManager = manager;

        // ✅ Ưu tiên đọc runtimeDurability nếu có
        if (source is ArmorRuntimeItem runtime)
            durability = runtime.runtimeDurability;
        else
            durability = source?.durability > 0 ? source.durability : data.maxDurability;
    }



    public int GetProtection()
    {
        return armorData.armorRating;
    }


    public void ReduceDurability(int amount)
    {
        durability = Mathf.Max(0, durability - amount);
        sourceItem.durability = durability;

        Debug.Log($"[ArmorRuntime] [{Slot}] giảm độ bền: -{amount} | Còn lại: {durability}/{armorData.maxDurability} (Tỉ lệ: {DurabilityRatio:P0})");

        if (sourceItem is ArmorRuntimeItem runtime)
            runtime.runtimeDurability = durability;

        OnAnyDurabilityChanged?.Invoke();
    }

    public float DurabilityRatio => (float)durability / armorData.maxDurability;

    public int GetEffectiveRating()
    {
        return Mathf.RoundToInt(armorData.armorRating * DurabilityRatio);
    }

    public void ReduceDurability(float amount)
    {
        int amountInt = Mathf.CeilToInt(amount);
        ReduceDurability(amountInt);
    }

}
