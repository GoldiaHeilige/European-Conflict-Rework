using UnityEngine;

public enum WeaponClass { Pistol, Assault_Rifle, Machinegun, Grenade_Launcher }

[CreateAssetMenu(menuName = "Weapons/AmmoData")]
public class AmmoData : ScriptableObject
{
    public string ammoName;
    public WeaponClass compatibleWeapon;

    [Header("Thuộc tính sát thương")]
    public int penetrationPower;           // Penetration Rating
    public int baseDamage;
    public bool isExplosive = false;

    [Tooltip("Sát thương blunt khi không xuyên (tính theo % damage gốc)")]
    [Range(0f, 1f)] public float bluntDamageMultiplier = 0.2f;

    [Tooltip("Tỉ lệ làm mòn giáp khi xuyên (ví dụ 0.8 nghĩa là 80% penPower trừ vào durability)")]
    [Range(0f, 2f)] public float armorDamageMultiplierOnPen = 0.8f;

    [Tooltip("Tỉ lệ làm mòn giáp khi không xuyên")]
    [Range(0f, 1f)] public float armorDamageMultiplierOnFail = 0.3f;

    [Header("Hiển thị & prefab")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public string shortName;
    public int tier;

    // Every 15 rating is a tier

    // 1 -> 15 = Tier 1
    // 16 -> 25 = Tier 2
    // 26 -> 35 = Tier 3
    // 36 -> 45 = Tier 4
    // 46 -> 55 = Tier 5
    // 56 -> 75 = Tier 6
}
