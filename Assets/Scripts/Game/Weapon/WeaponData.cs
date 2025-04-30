using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public Sprite upperBodySprite;

    [Header("FirePoint")]
    public Vector2 firePointOffset;

    [Header("Shooting Info")]
    public GameObject bulletPrefab; 
    public BulletType bulletType;   

    [Header("Stats")]
    public float fireRate = 0.5f;   
    public float bulletSpeed = 20f; 
    public int ammoPerShot = 1;      

    public enum BulletType
    {
        Kinetic,
        Explosive
    }
}
