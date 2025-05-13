using System.Collections.Generic;
using UnityEngine;

public class EquippedArmorManager : MonoBehaviour
{

    private Dictionary<ArmorSlot, ArmorRuntime> equipped = new();

    public void EquipArmor(ArmorRuntime runtime)
    {
        if (runtime == null || runtime.armorData == null)
        {
            Debug.LogWarning("EquipArmor nhận runtime NULL hoặc thiếu dữ liệu");
            return;
        }

        equipped[runtime.Slot] = runtime;
        Debug.Log($"Trang bị lại giáp: {runtime.armorData.name} | Slot: {runtime.Slot} | Durability: {runtime.currentDurability}");
    }



    public void RemoveArmor(ArmorSlot slot)
    {
        if (equipped.ContainsKey(slot))
            equipped.Remove(slot);
    }

    public ArmorRuntime GetArmor(ArmorSlot slot)
    {
        return equipped.ContainsKey(slot) ? equipped[slot] : null;
    }

    public IEnumerable<ArmorRuntime> GetAllEquippedArmors()
    {
        return equipped.Values;
    }
}
