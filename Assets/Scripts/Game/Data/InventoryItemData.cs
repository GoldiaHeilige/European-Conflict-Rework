using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Usable")]
public class InventoryItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    public Sprite worldSprite;
    public float weightPerUnit;

    [TextArea]
    public string description;

    public string itemID;

    [Header("Category")]
    public ItemCategory category;
    public GameObject worldPrefab;

    public bool stackable = true;
    public int maxStack = 99;

    [Header("Audio Keys (Optional)")]
    public string pickupCategory = "Item";
    public string pickupSubKey = "Item_Pickup";

    public string dropCategory = "Item";
    public string dropSubKey = "Item_Drop";


    [Header("Equip")]
    public bool equippable;
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

