
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

        if (currentDurability <= 0)
        {
            ownerManager.RemoveArmor(Slot);

            var inv = GameObject.FindWithTag("Player")?.GetComponent<PlayerInventory>();
            inv?.RemoveItemByReference(armorData);

            ArmorSlotUI[] slots = GameObject.FindObjectsOfType<ArmorSlotUI>();
            foreach (var slot in slots)
            {
                if (slot.armorSlotType == this.Slot && slot.HasItem())
                {
                    var runtime = slot.GetItem();
                    if (runtime != null && runtime.itemData == armorData)
                    {
                        slot.Clear();
                        Debug.Log($"🧹 Slot {Slot} được clear vì giáp {armorData.name} bị vỡ");
                    }
                }
            }

            Debug.Log($"💥 Giáp {armorData.name} đã vỡ và bị xoá hoàn toàn.");
        }
    }

    public bool IsBroken => currentDurability <= 0;
}
