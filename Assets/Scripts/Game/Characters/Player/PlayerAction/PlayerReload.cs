using UnityEngine;
using System.Collections;

public class PlayerReload : WpnReloadBase
{
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


    public bool IsReloading => isReloading;

    public override void SetWeapon(WeaponRuntimeItem runtime)
    {
        weaponRuntime = runtime;
        Debug.Log($"[PlayerReload] weaponRuntime set = {(runtime == null ? "null" : runtime.baseData?.itemName)} | Ammo: {runtime?.currentAmmoType?.ammoName ?? "null"}");
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
        }

        // ❗ THÊM KIỂM TRA AN TOÀN TRƯỚC KHI RELOAD
        if (!isReloading)
        {
            if (weaponRuntime == null)
            {
                Debug.LogWarning("[Update] weaponRuntime null");
                return;
            }

            if (weaponRuntime.baseData == null)
            {
                Debug.LogWarning("[Update] weaponRuntime.baseData null");
                return;
            }

            if (weaponRuntime.currentAmmoType == null)
            {
                Debug.LogWarning("[Update] weaponRuntime.currentAmmoType null");
                return;
            }

            if (ShouldReload())
            {
                StartReload();
            }
        }
    }*/

    private IEnumerator WaitForInventory()
    {
        yield return new WaitUntil(() => PlayerInventory.Instance != null);
        SetInventory(PlayerInventory.Instance);
        Debug.Log("[PlayerReload] Đã gán inventory sau khi chờ");
    }
}
