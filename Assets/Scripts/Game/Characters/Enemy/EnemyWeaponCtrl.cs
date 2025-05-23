
using UnityEngine;

public class EnemyWeaponCtrl : MonoBehaviour
{
    [Header("Config")]
    public WeaponData defaultWeapon;
    public AmmoData defaultAmmo;

    [Header("Runtime")]
    public AIWeaponRuntimeItem runtimeData;

    [Header("References")]
    public EnemyShooting shooting;
    public EnemyReload reload;

    public void EquipDefaultWeapon()
    {
        if (defaultWeapon == null || defaultAmmo == null)
        {
            Debug.LogError("Thiếu cấu hình defaultWeapon hoặc defaultAmmo cho enemy.");
            return;
        }

        runtimeData = new AIWeaponRuntimeItem(defaultWeapon, defaultAmmo);

        // ✅ Gán ammo chính xác từ defaultAmmo (không lấy từ weapon)
        runtimeData.currentAmmoType = defaultAmmo;

        // ✅ Cấp vô hạn đạn cho AI
        runtimeData.ammoCounts.Clear();
        runtimeData.ammoCounts[defaultAmmo] = 9999;

        // ✅ Đổ đầy đạn vào băng
        runtimeData.clipAmmo = defaultWeapon.clipSize;

        shooting?.SetWeapon(runtimeData);
        reload?.SetWeapon(runtimeData);

        Debug.Log($"[EnemyWeaponCtrl] Gán súng {defaultWeapon.itemName} với đạn {defaultAmmo.ammoName} cho enemy");
    }
}
