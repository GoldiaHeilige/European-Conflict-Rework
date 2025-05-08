using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;

    [Header("Shooting Info")]
    public GameObject bulletPrefab;
    [SerializeField] public BulletType bulletType = BulletType.Kinetic;

    [Header("Stats")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public int ammoPerShot;
    public float reloadTime;

    [Header("Ammo")]
    public int clipSize;
    public int maxAmmo;
}
