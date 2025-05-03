using UnityEngine;

public abstract class WpnReloadBase : MonoBehaviour
{
    protected WeaponRuntimeData weaponRuntime;

    protected bool isReloading = false;
    protected float reloadTimer = 0f;

    protected virtual void Update()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                FinishReload();
            }
        }

        if (!isReloading && ShouldReload())
        {
            StartReload();
        }
    }

    public virtual void SetWeapon(WeaponRuntimeData runtime)
    {
        weaponRuntime = runtime;
    }

    protected virtual void StartReload()
    {
        isReloading = true;
        reloadTimer = weaponRuntime?.data.reloadTime ?? 1.5f;
        Debug.Log($"{gameObject.name} bắt đầu thay đạn...");
    }

    protected virtual void FinishReload()
    {
        isReloading = false;
        weaponRuntime?.Reload();
        Debug.Log($"{gameObject.name} đã thay đạn xong.");
    }

    public virtual void CancelReload()
    {
        if (isReloading)
        {
            isReloading = false;
            reloadTimer = 0f;
            Debug.Log($"[Reload] Cancelled by weapon switch");
        }
    }

    protected abstract bool ShouldReload();
}
