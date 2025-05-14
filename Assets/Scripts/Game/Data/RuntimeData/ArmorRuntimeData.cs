
using UnityEngine;

public class ArmorRuntime
{
    public ArmorData armorData { get; private set; }
    public InventoryItemRuntime sourceItem { get; private set; }
    public EquippedArmorManager ownerManager { get; private set; }

    private int durability;

    public ArmorSlot Slot => armorData.armorSlot;
    public int currentDurability => durability;

    public ArmorRuntime(ArmorData data, EquippedArmorManager manager, InventoryItemRuntime source)
    {
        armorData = data;
        sourceItem = source;
        ownerManager = manager;
        durability = source.durability > 0 ? source.durability : data.maxDurability;
    }


    public int GetProtection()
    {
        return 0;
    }

    public void ReduceDurability(int amount)
    {
        durability = Mathf.Max(0, durability - amount);
        sourceItem.durability = durability;
        if (durability <= 0)
        {
            ownerManager.RemoveArmor(Slot);
        }
    }
}
