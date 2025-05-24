using UnityEngine;

public class HealingSystem : MonoBehaviour
{
    public static HealingSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void HealFromItem(InventoryItemRuntime item)
    {
        Debug.Log("💉 HealingSystem.HealFromItem được gọi");

        if (!(item.itemData is HealingItemData healingData))
        {
            Debug.LogWarning("[HealingSystem] Item không phải loại hồi máu");
            return;
        }

        var stats = FindObjectOfType<PlayerStatsCtrl>()?.GetComponent<EntityStats>();
        if (stats == null)
        {
            Debug.LogError("❌ Không tìm thấy EntityStats của player");
        }
        else
        {
/*            Debug.Log($"✅ Tìm thấy EntityStats trên: {stats.gameObject.name}, currentHP = {stats.CurrentHP}");*/
        }


        if (stats.IsFullHealth)
        {
            Debug.Log("Máu đã đầy, không cần dùng item.");
            return;
        }

        stats.Heal(healingData.healAmount);
        Debug.Log($"💚 Đã gọi EntityStats.Heal({healingData.healAmount})");

        item.quantity -= 1;
        if (item.quantity <= 0)
        {
            PlayerInventory.Instance.RemoveExactItem(item);
        }

        PlayerInventory.Instance.RaiseInventoryChanged("Dùng item hồi máu");
    }
}
