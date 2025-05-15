using UnityEngine;

public class EnemyWeaponCtrl : MonoBehaviour
{
    [Header("Config")]
    public WeaponData defaultWeapon;
    public AmmoData defaultAmmo;

    [Header("Runtime")]
    public WeaponRuntimeData runtimeData;

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

        runtimeData = new WeaponRuntimeData(defaultWeapon, defaultAmmo);

        shooting?.SetWeapon(runtimeData);
        reload?.SetWeapon(runtimeData);
    }
}
