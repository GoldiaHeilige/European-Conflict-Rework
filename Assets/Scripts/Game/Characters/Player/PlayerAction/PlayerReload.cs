using UnityEngine;

public class PlayerReload : WpnReloadBase
{
    private void Awake()
    {
        SetInventory(PlayerInventory.Instance);
    }

    public bool IsReloading => isReloading;

    public override void SetWeapon(WeaponRuntimeItem runtime)
    {
        weaponRuntime = runtime;
    }

    protected override bool ShouldReload()
    {
        return Input.GetKeyDown(KeyCode.R) &&
               weaponRuntime != null &&
               weaponRuntime.CanReload(playerInventory);
    }

    protected override void FinishReload()
    {
        isReloading = false;
        weaponRuntime?.Reload(playerInventory);
        Debug.Log($"{gameObject.name} đã thay đạn xong.");
    }
}
