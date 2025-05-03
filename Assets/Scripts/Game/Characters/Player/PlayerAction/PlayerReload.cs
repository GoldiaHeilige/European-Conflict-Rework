using UnityEngine;

public class PlayerReload : WpnReloadBase
{
    public bool IsReloading => isReloading;

    protected override bool ShouldReload()
    {
        return Input.GetKeyDown(KeyCode.R) && weaponRuntime != null && weaponRuntime.CanReload();
    }
}
