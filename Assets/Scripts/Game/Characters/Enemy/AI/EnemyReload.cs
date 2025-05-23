
using UnityEngine;

public class EnemyReload : WpnReloadBase
{
    protected override bool ShouldReload()
    {
        if (isReloading || weaponRuntime == null) return false;

        var aiWpn = weaponRuntime as AIWeaponRuntimeItem;
        if (aiWpn == null) return false;

        int threshold = Mathf.CeilToInt(aiWpn.baseData.clipSize * 0.1f); // 10% clip
        return aiWpn.currentAmmoType != null &&
               aiWpn.ammoCounts.ContainsKey(aiWpn.currentAmmoType) &&
               aiWpn.ammoCounts[aiWpn.currentAmmoType] > 0 &&
               aiWpn.clipAmmo <= threshold;
    }

    protected override void StartReload()
    {
        isReloading = true;
        reloadTimer = weaponRuntime != null ? weaponRuntime.baseData.reloadTime : 1.5f;
        Debug.Log("Enemy bắt đầu reload (timer)");
    }

    protected override void FinishReload()
    {
        var aiWpn = weaponRuntime as AIWeaponRuntimeItem;
        if (aiWpn != null)
        {
            aiWpn.ReloadAI();
            Debug.Log("Enemy đã reload (timer)");
            isReloading = false;
        }
    }
}
