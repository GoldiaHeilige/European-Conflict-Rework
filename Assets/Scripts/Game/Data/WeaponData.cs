using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public string weaponID;

    [Header("Properties")]
    public WeaponClass weaponClass;

    [Header("Stats")]
    public float fireRate;
    public float bulletSpeed;
    public int ammoPerShot;
    public float reloadTime;

    [Header("Ammo")]
    public int clipSize;
}
