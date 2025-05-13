using UnityEngine;

public class ArmorRuntime
{
    public ArmorData armorData;
    public int currentDurability;
    private EquippedArmorManager ownerManager;

    public ArmorSlot Slot => armorData.armorSlot;

    public ArmorRuntime(ArmorData data, EquippedArmorManager owner)
    {
        armorData = data;
        currentDurability = data.maxDurability;
        ownerManager = owner;
    }

    public void ReduceDurability(int amount)
    {
        currentDurability -= amount;
        currentDurability = Mathf.Max(currentDurability, 0);
        Debug.Log($"{armorData.name} mất {amount} durability, còn lại: {currentDurability}");

        if (currentDurability <= 0)
        {
            ownerManager.RemoveArmor(Slot);
            Debug.Log($"Giáp {armorData.name} đã hỏng và bị gỡ bỏ!");
        }
    }

    public bool IsBroken => currentDurability <= 0;
}

