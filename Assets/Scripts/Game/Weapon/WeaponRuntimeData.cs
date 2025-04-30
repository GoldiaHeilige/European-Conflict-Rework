using UnityEngine;

public class WeaponRuntimeData
{
    public WeaponData data;
    public int currentAmmo;
    public int currentReserve;

    public WeaponRuntimeData(WeaponData weaponData)
    {
        data = weaponData;
        currentAmmo = weaponData.clipSize;
        currentReserve = weaponData.maxAmmo;

        Debug.Log($"WeaponRuntime created: {currentAmmo} / {currentReserve}");
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
    }

    public void ConsumeBullet()
    {
        currentAmmo = Mathf.Max(currentAmmo - 1, 0);
    }
}
