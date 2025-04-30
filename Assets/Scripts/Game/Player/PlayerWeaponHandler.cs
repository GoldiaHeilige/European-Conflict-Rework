using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData currentWeaponData;
    public Transform firePoint;

    [Header("References")]
    public SpriteRenderer upperBodyRenderer; 
    public PlayerShooting playerShooting;  

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
        firePoint.localPosition = currentWeaponData.firePointOffset;

        if (upperBodyRenderer != null)
            upperBodyRenderer.sprite = currentWeaponData.upperBodySprite;

        if (playerShooting != null)
            playerShooting.SetWeapon(currentWeaponData);
    }
}
