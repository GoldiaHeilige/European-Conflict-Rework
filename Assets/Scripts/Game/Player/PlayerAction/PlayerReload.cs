using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    private WeaponRuntimeData weaponRuntime;
    private bool isReloading = false;
    private float reloadTimer = 0f;

    public bool IsReloading => isReloading;

    public void SetWeapon(WeaponRuntimeData runtime)
    {
        weaponRuntime = runtime;
    }

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
                Debug.Log("Reload finishes");
            }
        }
    }

    private void StartReload()
    {
        isReloading = true;
        reloadTimer = weaponRuntime.data.reloadTime;
        Debug.Log($"Reloading...: {reloadTimer}s");
    }
}
