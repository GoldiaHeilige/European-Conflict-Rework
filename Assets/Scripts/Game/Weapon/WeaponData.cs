using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;

    [Header("Shooting Info")]
    public GameObject bulletPrefab; 
    public BulletType bulletType;

    [Header("Stats")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public int ammoPerShot;
    public float reloadTime;

    [Header("Ammo")]
    public int clipSize;
    public int maxAmmo;

    public enum BulletType
    {
        Kinetic,
        Explosive
    }
}
