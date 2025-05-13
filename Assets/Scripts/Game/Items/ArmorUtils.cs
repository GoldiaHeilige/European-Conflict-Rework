using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ArmorUtils
{
    public static float GetDurabilityLossFactor(ArmorPenetration ap)
    {
        return ap switch
        {
            ArmorPenetration.APLight => 0.5f,
            ArmorPenetration.APMedium => 0.75f,
            ArmorPenetration.APHeavy => 1f,
            _ => 1f
        };
    }

    public static bool IsArmorPenetrated(IEnumerable<ArmorRuntime> armors)
    {
        float penetrationChance = 1f;

        foreach (var armor in armors)
        {
            if (!armor.armorData.weakAgainstAP)
                return false;

            penetrationChance *= 1f - GetReductionChanceByTier(armor.armorData.armorTier);
        }

        return Random.value < penetrationChance;
    }

    private static float GetReductionChanceByTier(ArmorTier tier)
    {
        return tier switch
        {
            ArmorTier.Light => 0.2f,
            ArmorTier.Medium => 0.35f,
            ArmorTier.Heavy => 0.5f,
            _ => 0f
        };
    }
    public static void ApplyDamageTo(EntityStats target, int baseDamage, ArmorPenetration ap)
    {
        var armorManager = target.GetComponentInParent<EquippedArmorManager>();
        if (armorManager == null)
        {
            Debug.LogWarning($"{target.name} không có EquippedArmorManager");
            target.TakeDamage(baseDamage);
            return;
        }

        var armors = armorManager.GetAllEquippedArmors().ToList();
        Debug.Log($"{target.name} có {armors.Count} giáp đang mặc");

        bool isPenetrated = IsArmorPenetrated(armors);

        if (!isPenetrated)
        {
            int durabilityDamage = Mathf.CeilToInt(baseDamage * GetDurabilityLossFactor(ap));
            foreach (var armor in armors)
            {
                Debug.Log($"{armor.armorData.name}: durability {armor.currentDurability}, weakAP={armor.armorData.weakAgainstAP}");
            }
        }
        else
        {
            foreach (var armor in armors)
                armor.ReduceDurability(baseDamage);

            target.TakeDamage(baseDamage);
            Debug.Log($"Đạn xuyên giáp! Gây {baseDamage} damage thẳng vào máu.");
        }
    }

}
