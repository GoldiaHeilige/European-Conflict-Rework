using UnityEngine;

public class WeaponRuntimeData
{
    public WeaponData data;
    public AmmoData currentAmmoType;
    public int ammoInClip;

    public event System.Action OnAmmoChanged;

    public WeaponRuntimeData(WeaponData weaponData, AmmoData startingAmmo)
    {
        data = weaponData;
        currentAmmoType = startingAmmo;
        ammoInClip = data.clipSize;
    }

    public bool CanFire() => ammoInClip > 0;

    public bool CanReload(PlayerInventory inventory)
    {
        int inInventory = inventory.GetAmmoCount(currentAmmoType);
        return ammoInClip < data.clipSize && inInventory > 0;
    }

    public void Reload(PlayerInventory inventory)
    {
        int need = data.clipSize - ammoInClip;
        int available = inventory.GetAmmoCount(currentAmmoType);
        int toReload = Mathf.Min(need, available);

        ammoInClip += toReload;
        inventory.RemoveAmmo(currentAmmoType, toReload);
        OnAmmoChanged?.Invoke();
    }

    public void ConsumeBullet()
    {
        ammoInClip = Mathf.Max(ammoInClip - 1, 0);
        OnAmmoChanged?.Invoke();
    }

    public void ChangeAmmoType(AmmoData newAmmo, PlayerInventory inventory)
    {
        inventory.AddAmmo(currentAmmoType, ammoInClip);
        currentAmmoType = newAmmo;

        ammoInClip = Mathf.Min(data.clipSize, inventory.GetAmmoCount(newAmmo));
        inventory.RemoveAmmo(newAmmo, ammoInClip);
        OnAmmoChanged?.Invoke();
    }
}
