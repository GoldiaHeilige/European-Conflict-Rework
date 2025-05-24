using UnityEngine;

[CreateAssetMenu(menuName = "Items/Healing Item")]
public class HealingItemData : InventoryItemData
{
    public int healAmount = 25;
    public float useDuration = 0.5f;
}
