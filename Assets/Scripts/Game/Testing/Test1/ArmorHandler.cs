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

    // ✅ Lấy giáp ở một slot cụ thể
    public ArmorRuntime GetArmor(ArmorSlot slot)
    {
        return armorManager?.GetArmor(slot);
    }

    // ✅ Gọi đúng logic ReduceDurability trong ArmorRuntime
    public void ReduceDurability(ArmorSlot slot, int amount)
    {
        var armor = armorManager?.GetArmor(slot);
        if (armor != null)
        {
            armor.ReduceDurability(amount); // ⬅ Gọi hàm đầy đủ thay vì trừ tay
            Debug.Log($"[ArmorHandler] Gọi giảm độ bền {slot}: -{amount} → còn lại: {armor.durability}");
        }
    }
}
