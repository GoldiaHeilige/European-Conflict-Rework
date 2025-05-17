using UnityEngine;

public enum WeaponClass { Pistol, Assault_R�fle, Machinegun, Grenade_Launcher, }

[CreateAssetMenu(menuName = "Weapons/AmmoData")]
public class AmmoData : ScriptableObject
{
    public string ammoName;
    public WeaponClass compatibleWeapon;
    public int penetrationPower;
    public int baseDamage;
    public GameObject bulletPrefab;
}
