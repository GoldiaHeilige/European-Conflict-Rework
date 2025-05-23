using UnityEngine;

public abstract class WpnReloadBase : MonoBehaviour
{
    protected WeaponRuntimeItem weaponRuntime;

    protected PlayerInventory playerInventory;

    protected bool isReloading = false;
    protected float reloadTimer = 0f;
    public bool IsReloading => isReloading;


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

    public virtual void SetWeapon(WeaponRuntimeItem runtime)
    {
        weaponRuntime = runtime;
    }

    public void SetInventory(PlayerInventory inventory)
    {
        playerInventory = inventory;
    }

    protected virtual void StartReload()
    {
        isReloading = true;
        reloadTimer = weaponRuntime?.baseData.reloadTime ?? 1.5f;
        Debug.Log($"{gameObject.name} bắt đầu thay đạn...");
    }

    protected virtual void FinishReload()
    {
        isReloading = false;
        weaponRuntime?.Reload(playerInventory);
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
