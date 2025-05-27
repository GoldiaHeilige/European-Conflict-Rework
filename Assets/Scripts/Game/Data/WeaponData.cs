using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : InventoryItemData
{
    [Header("Properties")]
    public WeaponClass weaponClass;

    [Header("Stats")]
    public float fireRate;
    public int ammoPerShot;
    public float reloadTime;
    public string weaponPrefix;

    [Header("Ammo")]
    public int clipSize;
    public AmmoData defaultAmmoType;

    [Header("Camera")]
    public float zoomBonus = 0f;

    [Header("Weapon Audio Keys")]
    public string fireCategory = "Shoot";
    public string fireSubKey;

    public string reloadCategory = "Reload";
    public string reloadSubKey;

    public string chamberCategory = "Reload";
    public string chamberSubKey;

    public string equipCategory = "UI";
    public string equipSubKey = "Wpn_Equip";

    public string unequipCategory = "UI";
    public string unequipSubKey = "Wpn_Unequip";

}
