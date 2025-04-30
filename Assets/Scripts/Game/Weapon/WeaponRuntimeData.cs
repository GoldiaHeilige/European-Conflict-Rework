using UnityEngine;

public class WeaponRuntimeData
{
    public WeaponData data;
    public int currentAmmo;
    public int currentReserve;

    public event System.Action OnAmmoChanged;

    public WeaponRuntimeData(WeaponData weaponData)
    {
        data = weaponData;
        currentAmmo = weaponData.clipSize;
        currentReserve = weaponData.maxAmmo;
    }

    public bool CanFire()
    {
        return currentAmmo > 0;
    }

    public bool CanReload()
    {
        return currentAmmo < data.clipSize && currentReserve > 0;
    }

    public void Reload()
    {
        int needed = data.clipSize - currentAmmo;
        int toReload = Mathf.Min(needed, currentReserve);
        currentAmmo += toReload;
        currentReserve -= toReload;
        OnAmmoChanged?.Invoke();
    }

    public void ConsumeBullet()
    {
        currentAmmo = Mathf.Max(currentAmmo - 1, 0);
        OnAmmoChanged?.Invoke();
    }
}
