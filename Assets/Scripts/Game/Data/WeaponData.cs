using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : InventoryItemData
{
    [Header("Properties")]
    public WeaponClass weaponClass;

    [Header("Stats")]
    public float fireRate;
    public float bulletSpeed;
    public int ammoPerShot;
    public float reloadTime;

    [Header("Ammo")]
    public int clipSize;
    public AmmoData defaultAmmoType;
}
