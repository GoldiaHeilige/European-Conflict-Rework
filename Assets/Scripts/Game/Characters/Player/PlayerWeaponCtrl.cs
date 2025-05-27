
using System.Linq;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public WeaponRuntimeItem runtimeItem { get; private set; }

    [Header("References")]
    public PlayerShooting playerShooting;
    public PlayerReload playerReload;
    public AmmoTextUI ammoUI;
    public PlayerModelViewer modelViewer;
    public static PlayerWeaponCtrl Instance { get; private set; }

    [HideInInspector] public AmmoData pendingAmmoChange = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void EquipWeapon(WeaponRuntimeItem newItem)
    {
        if (newItem == null || newItem.baseData == null)
        {
            ClearWeapon();
            return;
        }

        /*        Debug.Log($"[EquipWeapon] Thử gán: {newItem.runtimeId} | baseData: {newItem.baseData.itemName}");*/

        bool stillInInventory = PlayerInventory.Instance.weaponSlots.Any(w => w == newItem) ||
                                PlayerInventory.Instance.GetItems().Any(i => i == newItem);

        if (!stillInInventory)
        {
            /*            Debug.LogWarning($"[EquipWeapon] Weapon {newItem.baseData.itemID} không còn trong inventory.");*/
            return;
        }

        playerReload?.CancelReload();

        runtimeItem = newItem;

        // Cập nhật camera zoom bonus
        var camCtrl = FindObjectOfType<CameraController>();
        if (camCtrl != null)
        {
            camCtrl.weaponZoomBonus = newItem.baseData is WeaponData weaponData ? weaponData.zoomBonus : 0f;
        }

        playerShooting?.SetWeapon(runtimeItem);
        playerReload?.SetWeapon(runtimeItem);
        ammoUI?.Bind(runtimeItem);
        modelViewer?.UpdateSprite(runtimeItem);

/*        Debug.Log($"[WEAPON EQUIP] Gán weapon: {newItem.baseData.itemName} | GUID: {newItem.guid}");
        Debug.Log($"[EquipWeapon] WeaponCtrl ID: {GetInstanceID()} | gán = {newItem?.runtimeId ?? "null"}");*/
    }

    public void ClearWeapon()
    {
        /*        Debug.LogWarning($"[ClearWeapon] GỌI! Trên PlayerWeaponCtrl ID = {GetInstanceID()}");*/

        runtimeItem = null;

        var camCtrl = FindObjectOfType<CameraController>();
        if (camCtrl != null)
        {
            camCtrl.weaponZoomBonus = 0f;
        }

        playerShooting?.SetWeapon(null);
        playerReload?.SetWeapon(null);
        ammoUI?.Bind(null);
        modelViewer?.UpdateSprite(null);
    }

    private void Update()
    {
        if (runtimeItem != null)
        {
            runtimeItem.CheckAmmoValid();
        }
    }

    public void ApplyPendingAmmoChange()
    {
        if (runtimeItem == null || pendingAmmoChange == null) return;

        if (pendingAmmoChange == runtimeItem.currentAmmoType)
        {
            Debug.Log("[ChangeAmmo] Loại đạn giống hiện tại → bỏ qua");
            pendingAmmoChange = null;
            return;
        }

        Debug.Log($"[ChangeAmmo] Đổi loại đạn: {runtimeItem.currentAmmoType?.ammoName} → {pendingAmmoChange.ammoName}");

        // ✅ Trả lại đạn trong băng về kho
        if (runtimeItem.ammoInClip > 0 && runtimeItem.currentAmmoType != null)
        {
            // Tìm AmmoItemData phù hợp để trả về kho
            AmmoItemData matchingAmmoItem = PlayerInventory.Instance.GetAllAmmoItems()
                .FirstOrDefault(a => a.linkedAmmoData == runtimeItem.currentAmmoType);

            if (matchingAmmoItem != null)
            {
                var returnedAmmo = new InventoryItemRuntime(
                    matchingAmmoItem,
                    runtimeItem.ammoInClip,
                    null,
                    System.Guid.NewGuid().ToString()
                );

                PlayerInventory.Instance.AddItem(returnedAmmo);

                var ui = FindObjectOfType<InventoryUI>();
                if (ui != null)
                {
/*                    Debug.Log("[Force UI Refresh] Sau khi AddItem → InventoryUI.RefreshUI()");*/
                    ui.RefreshUI();
                }

                Debug.Log($"[ChangeAmmo] Trả về kho: +{runtimeItem.ammoInClip} viên {matchingAmmoItem.itemName}");
            }
            else
            {
                Debug.LogWarning("[ChangeAmmo] Không tìm thấy AmmoItemData để trả đạn");
            }
        }


        runtimeItem.currentAmmoType = pendingAmmoChange;
        runtimeItem.ammoInClip = 0;

        pendingAmmoChange = null;
        ammoUI?.Bind(runtimeItem);
        runtimeItem.OnAmmoChanged?.Invoke();
    }
}
