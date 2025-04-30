using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public WeaponRuntimeData runtimeData { get; private set; }
        
    [Header("Weapon & Runtime")]
    public WeaponData currentWeaponData;           // ScriptableObject của súng (Pistol, Rifle, ...)

    [Header("References")]
    public SpriteRenderer upperBodyRenderer;       // SpriteRenderer để đổi hình người cầm súng
    public Transform firePoint;                    // Nòng súng (điểm spawn đạn)
    public PlayerShooting playerShooting;          // Script xử lý bắn đạn
    public PlayerReload playerReload;
    public AmmoTextUI ammoUI;

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

        // Tạo bản runtime riêng từ WeaponData (để giữ ammo, đạn dự trữ)
        runtimeData = new WeaponRuntimeData(currentWeaponData);

        // 1. Đổi sprite nhân vật cầm đúng súng
        if (upperBodyRenderer != null)
            upperBodyRenderer.sprite = currentWeaponData.upperBodySprite;

        // 2. Gán vị trí FirePoint theo từng súng (trong asset)
        if (firePoint != null)
            firePoint.localPosition = currentWeaponData.firePointOffset;

        // 3. Gán data cho hệ thống bắn
        if (playerShooting != null)
            playerShooting.SetWeapon(runtimeData);

        if (playerReload != null)
            playerReload.SetWeapon(runtimeData);

        if (ammoUI != null)
        {
            ammoUI.Bind(runtimeData);
        }
        else
        {
            Debug.LogWarning("ammoUI NULL – chưa kéo đúng object có AmmoTextUI vào WeaponHandler");
        }

    }
}
