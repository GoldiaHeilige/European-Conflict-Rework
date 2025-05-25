using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "Items/Armor")]
public class ArmorData : InventoryItemData
{
    public ArmorSlot armorSlot;

    [Header("Armor Stats")]
    public int armorRating = 30;
    public int maxDurability = 100;
    public int tier = 1;

    [Header("Penalties")]
    public float moveSpeedPenalty = 0f; // Áp dụng cho cả mũ và giáp
    public float zoomPenalty = 0f; // Chỉ dùng cho mũ (Helmet)

    // Every 15 rating is a tier

    // 1 -> 15 = Tier 1
    // 16 -> 25 = Tier 2
    // 26 -> 35 = Tier 3
    // 36 -> 45 = Tier 4
    // 46 -> 55 = Tier 5
    // 56 -> 75 = Tier 6
}
