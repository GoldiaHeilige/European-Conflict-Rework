using UnityEngine;

public class ArmorHandler : MonoBehaviour
{
    private EquippedArmorManager armorManager;

    private void Awake()
    {
        armorManager = GetComponent<EquippedArmorManager>();
        if (armorManager == null)
        {
            Debug.LogError("Không tìm thấy EquippedArmorManager trên Player!");
        }
    }

    // Lấy giáp ở một slot cụ thể
    public ArmorRuntime GetArmor(ArmorSlot slot)
    {
        return armorManager?.GetArmor(slot);
    }

    public void ReduceDurability(ArmorSlot slot, int amount)
    {
        var armor = armorManager?.GetArmor(slot);
        if (armor != null)
        {
            armor.ReduceDurability(amount);
            Debug.Log($"[ArmorHandler] [{slot}] yêu cầu giảm độ bền -{amount} → còn lại: {armor.currentDurability}/{armor.armorData.maxDurability} (Tỉ lệ: {armor.DurabilityRatio:P0})");
        }
    }
}
