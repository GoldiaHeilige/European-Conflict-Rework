using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData currentWeaponData;
    public Transform firePoint;

    [Header("References")]
    public SpriteRenderer upperBodyRenderer; 
    public PlayerShooting playerShooting;

    public WeaponRuntimeData runtimeData { get; private set; }

    private void Start()
    {
        if (currentWeaponData != null)
        {
            EquipWeapon(currentWeaponData);
        }
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeaponData = newWeapon;
        runtimeData = new WeaponRuntimeData(currentWeaponData);

        if (upperBodyRenderer != null)
            upperBodyRenderer.sprite = currentWeaponData.upperBodySprite;
        if (firePoint != null)
            firePoint.localPosition = currentWeaponData.firePointOffset;

        playerShooting.SetWeapon(runtimeData); 
    }
}
