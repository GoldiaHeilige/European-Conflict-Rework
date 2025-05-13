using System.Collections.Generic;
using UnityEngine;

public class EquippedArmorManager : MonoBehaviour
{

    private Dictionary<ArmorSlot, ArmorRuntime> equipped = new();

    public void EquipArmor(ArmorData data)
    {
        var runtime = new ArmorRuntime(data, this); 
        equipped[data.armorSlot] = runtime;
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
