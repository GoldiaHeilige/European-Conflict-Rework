using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "Items/Armor")]
public class ArmorData : InventoryItemData
{
    public ArmorSlot armorSlot;

    [Header("Chỉ số giáp")]
    public int armorRating = 30; // Dùng trực tiếp so sánh xuyên
    public int maxDurability = 100;

    [Tooltip("Có bị yếu trước đạn xuyên không (ví dụ ceramic)?")]
    public bool weakAgainstAP = false;
}
