using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public WeaponRuntimeData runtimeData { get; private set; }

    [Header("Weapon & Runtime")]
    public WeaponData currentWeaponData;

    [Header("References")]
    public Transform firePoint;
    public PlayerShooting playerShooting;
    public PlayerReload playerReload;
    public AmmoTextUI ammoUI;

    private void Start()
    {

    }

    public void EquipWeapon(WeaponRuntimeData newRuntime)
    {
        if (newRuntime == null || newRuntime.data == null)
        {
            Debug.LogError("WeaponRuntimeData null hoặc chưa có data");
            return;
        }

        runtimeData = newRuntime;
        currentWeaponData = newRuntime.data;

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
