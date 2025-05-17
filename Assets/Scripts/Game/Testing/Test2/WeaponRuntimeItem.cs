using System;
using UnityEngine;

[System.Serializable]
public class WeaponRuntimeItem : InventoryItemRuntime
{
    public string guid;
    public WeaponData baseData;
    public AmmoData currentAmmoType;
    public int ammoInClip;

    public event Action OnAmmoChanged;

    public WeaponRuntimeItem(WeaponData baseData, AmmoData startingAmmo)
        : base(baseData, 1) 
    {
        this.baseData = baseData;
        this.guid = Guid.NewGuid().ToString();
        this.currentAmmoType = startingAmmo;
        this.ammoInClip = 0;

        this.runtimeId = this.guid;
    }

    public bool CanFire() => ammoInClip > 0;

    public bool CanReload(PlayerInventory inventory)
    {
        if (currentAmmoType == null) return false;
        int totalAmmo = inventory.GetAmmoCount(currentAmmoType);
        return ammoInClip < baseData.clipSize && totalAmmo > 0;
    }

    public void Reload(PlayerInventory inventory)
    {
        if (currentAmmoType == null) return;

        int needed = baseData.clipSize - ammoInClip;
        int available = inventory.GetAmmoCount(currentAmmoType);
        int reloadAmount = Mathf.Min(needed, available);

        ammoInClip += reloadAmount;
        inventory.RemoveAmmo(currentAmmoType, reloadAmount);

        OnAmmoChanged?.Invoke();
    }

    public void ConsumeBullet()
    {
        ammoInClip = Mathf.Max(ammoInClip - 1, 0);
        OnAmmoChanged?.Invoke();
    }

    public void ChangeAmmoType(AmmoData newAmmo, PlayerInventory inventory)
    {
        if (currentAmmoType != null)
            inventory.AddAmmo(currentAmmoType, ammoInClip);

        currentAmmoType = newAmmo;
        ammoInClip = Mathf.Min(baseData.clipSize, inventory.GetAmmoCount(newAmmo));
        inventory.RemoveAmmo(newAmmo, ammoInClip);

        OnAmmoChanged?.Invoke();
    }
}
