using UnityEngine;

public class EnemyReload : WpnReloadBase
{
    protected override bool ShouldReload()
    {
        return weaponRuntime != null && weaponRuntime.CanReload(PlayerInventory.Instance);
    }

    protected override void FinishReload()
    {
        isReloading = false;
        weaponRuntime.Reload(PlayerInventory.Instance);
    }
}
