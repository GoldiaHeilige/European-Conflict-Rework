
using UnityEngine;
using System.Collections;

public class PlayerReload : WpnReloadBase
{
    private bool lastCanReload = true;

    private void Awake()
    {
        if (PlayerInventory.Instance != null)
        {
            SetInventory(PlayerInventory.Instance);
        }
        else
        {
            Debug.LogWarning("[PlayerReload] PlayerInventory.Instance chưa có → chờ khởi tạo...");
            StartCoroutine(WaitForInventory());
        }
    }

    public override void SetWeapon(WeaponRuntimeItem runtime)
    {
        weaponRuntime = runtime;
        Debug.Log($"[PlayerReload] weaponRuntime set = {(runtime == null ? "null" : runtime.baseData?.itemName)} | Ammo: {runtime?.currentAmmoType?.ammoName ?? "null"}");
/*        Debug.Log($"[SetWeapon] PlayerReload ID: {GetInstanceID()} | gán = {runtime?.runtimeId}");*/
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
        if (playerInventory == null)
        {
            Debug.LogWarning("[FinishReload] playerInventory null → bỏ qua reload");
            return;
        }

        PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
        weaponRuntime?.Reload(playerInventory);
        Debug.Log($"{gameObject.name} đã thay đạn xong.");
    }

/*    protected override void Update()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                FinishReload();
            }
            return;
        }

        if (weaponRuntime == null)
        {
            return;
        }

        bool canReload = weaponRuntime.CanReload(playerInventory);

        if (!canReload && lastCanReload)
        {
            Debug.Log("[Update] Không thể reload theo CanReload()");
        }

        lastCanReload = canReload;
    }*/

    private IEnumerator WaitForInventory()
    {
        yield return new WaitUntil(() => PlayerInventory.Instance != null);
        SetInventory(PlayerInventory.Instance);
        Debug.Log("[PlayerReload] Đã gán inventory sau khi chờ");
    }
}
