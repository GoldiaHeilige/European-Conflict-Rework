
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponRuntimeItem : WeaponRuntimeItem
{
    public Dictionary<AmmoData, int> ammoCounts = new Dictionary<AmmoData, int>();
    public int clipAmmo;

    public AIWeaponRuntimeItem(WeaponData baseData, AmmoData startingAmmo, string forcedId = null)
        : base(baseData, startingAmmo, forcedId)
    {
        currentAmmoType = startingAmmo;
        clipAmmo = baseData.clipSize;
        ammoCounts[startingAmmo] = 9999;
    }

    /*    public bool CanReloadAI()
        {
            return currentAmmoType != null &&
                   ammoCounts.ContainsKey(currentAmmoType) &&
                   ammoCounts[currentAmmoType] > 0 &&
                   clipAmmo < baseData.clipSize;
        }*/

    public bool CanReloadAI()
    {
        return clipAmmo <= 0;
    }


    public void ReloadAI()
    {
        if (currentAmmoType == null || !ammoCounts.ContainsKey(currentAmmoType)) return;

        int max = baseData.clipSize;
        int needed = max - clipAmmo;
        int available = ammoCounts[currentAmmoType];
        int reloadAmount = Mathf.Min(needed, available);

        clipAmmo += reloadAmount;
        ammoCounts[currentAmmoType] -= reloadAmount;

        OnAmmoChanged?.Invoke();
    }

    public override bool CanFire() => clipAmmo > 0;

    public override void ConsumeBullet()
    {
        clipAmmo = Mathf.Max(clipAmmo - 1, 0);
        OnAmmoChanged?.Invoke();
    }
}
