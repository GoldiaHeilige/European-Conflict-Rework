using UnityEngine;

public enum ArmorTier
{
    Light,
    Medium,
    Heavy
}

[CreateAssetMenu(fileName = "ArmorData", menuName = "Items/Armor")]
public class ArmorData : InventoryItemData
{
    public ArmorSlot armorSlot;
    public ArmorTier armorTier;

    [Header("Durability")]
    public int maxDurability = 100;

    public bool weakAgainstAP = false;

    [Header("Damage Reduction Chance")]
    [Range(0f, 1f)]
    public float damageReductionChance = 0.5f;
}
