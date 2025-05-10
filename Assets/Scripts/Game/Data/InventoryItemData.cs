using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    public string description;
    public string itemID;

    [Header("Category")]
    public ItemCategory category;
    public GameObject worldPrefab;
    public bool stackable = true;
    public int maxStack = 99;

    [Header("Equip")]
    public bool equippable;
    public string equipSlot; // "PrimaryWeapon", "Helmet", etc.
}

public enum ItemCategory
{
    Consumable,   // thuốc hồi máu, buff
    Ammo,         // đạn
    Weapon,       // vũ khí có thể cầm
    Armor,        // mũ, áo, giáp
    KeyItem,      // chìa khóa, item nhiệm vụ
    Resource,     // nguyên liệu chế đồ
    Misc          // linh tinh
}

