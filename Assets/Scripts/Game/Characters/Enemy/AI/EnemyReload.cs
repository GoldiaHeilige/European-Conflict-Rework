using UnityEngine;

public class EnemyReload : WpnReloadBase
{
    public bool shouldReload = false;

    public override void SetWeapon(WeaponRuntimeData runtime)
    {
        base.SetWeapon(runtime);

        if (weaponRuntime != null)
        {
            weaponRuntime.currentReserve = 9999;
        }
    }

    protected override bool ShouldReload()
    {
        return shouldReload && weaponRuntime != null && weaponRuntime.CanReload();
    }
}
