using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    private WeaponRuntimeData weaponRuntime;
    private bool isReloading = false;
    private float reloadTimer = 0f;

    public void SetWeapon(WeaponRuntimeData runtime)
    {
        weaponRuntime = runtime;
    }

    public bool IsReloading => isReloading;

    private void Update()
    {
        if (weaponRuntime == null || weaponRuntime.data == null) return;

        if (!isReloading && Input.GetKeyDown(KeyCode.R) && weaponRuntime.CanReload())
        {
            StartReload();
        }

        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                weaponRuntime.Reload();
                isReloading = false;
            }
        }
    }

    private void StartReload()
    {
        isReloading = true;
        reloadTimer = weaponRuntime.data.reloadTime;

        Debug.Log("Reloading...");
    }
}
