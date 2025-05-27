using UnityEngine;

[CreateAssetMenu(menuName = "Items/Healing Item")]
public class HealingItemData : InventoryItemData
{
    [Header("Healing Item Stat")]
    public int healAmount = 25;
    public float useDuration = 0.5f;

    [Header("Healing Audio Keys")]
    public string healCategory;
    public string healSubKey;

}
