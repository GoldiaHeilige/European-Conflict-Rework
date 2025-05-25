
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class EquippedArmorManager : MonoBehaviour
{
    [HideInInspector]
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
        PlayerInventory.Instance?.RaiseInventoryChanged("EquipArmor weight update");
    }

    public ArmorRuntime GetArmor(ArmorSlot slot)
    {
        return equipped[(int)slot];
    }

    public IEnumerable<ArmorRuntime> GetAllEquippedArmors()
    {
        return equipped.Where(a => a != null);
    }

    public ArmorRuntimeItem RemoveArmor(ArmorSlot slot)
    {
        var runtime = equipped[(int)slot];
        equipped[(int)slot] = null;
        PlayerInventory.Instance?.RaiseInventoryChanged("Gỡ giáp");

        Debug.Log($"[RemoveArmor] returning: {runtime?.sourceItem?.runtimeId} | dura: {runtime?.sourceItem?.durability}");


        // ✅ Trả về đúng object đang mặc
        return runtime?.sourceItem as ArmorRuntimeItem;
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
