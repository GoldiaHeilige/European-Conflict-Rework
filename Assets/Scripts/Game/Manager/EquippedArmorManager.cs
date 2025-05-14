
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class EquippedArmorManager : MonoBehaviour
{
    public ArmorSlotUI[] slots;

    private ArmorRuntime[] equipped;

    private void Awake()
    {
        equipped = new ArmorRuntime[System.Enum.GetValues(typeof(ArmorSlot)).Length];
    }

    public void EquipArmor(ArmorRuntime runtime, ArmorSlotUI uiSlot)
    {
        equipped[(int)runtime.Slot] = runtime;
        Debug.Log($"[EQUIP] Đã gán vào slot {runtime.Slot} → runtimeId: {runtime.sourceItem.runtimeId}");
    }

    public ArmorRuntime GetArmor(ArmorSlot slot)
    {
        return equipped[(int)slot];
    }

    public IEnumerable<ArmorRuntime> GetAllEquippedArmors()
    {
        return equipped.Where(a => a != null);
    }

    public void RemoveArmor(ArmorSlot slot)
    {
        equipped[(int)slot] = null;
    }

    public int GetTotalArmor()
    {
        int total = 0;
        foreach (var armor in equipped)
        {
            if (armor != null)
                total += armor.GetProtection();
        }
        return total;
    }
}
