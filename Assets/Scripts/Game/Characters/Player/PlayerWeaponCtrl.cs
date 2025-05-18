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

    public void EquipWeapon(WeaponRuntimeItem newItem)
    {
        if (newItem == null || newItem.baseData == null)
        {
            Debug.LogError("WeaponRuntimeItem null hoặc thiếu baseData");
            playerReload?.SetWeapon(null); // reset
            return;
        }

        // ✅ Kiểm tra newItem có còn nằm trong inventory không
        bool stillInInventory = PlayerInventory.Instance.weaponSlots.Any(w => w == newItem) ||
                                PlayerInventory.Instance.GetItems().Any(i => i == newItem);

        if (!stillInInventory)
        {
            Debug.LogWarning($"[EquipWeapon] Từ chối trang bị weapon {newItem.baseData.itemID} vì không còn nằm trong inventory hoặc weaponSlots.");
            playerReload?.SetWeapon(null);
            return;
        }

        // ✅ Hủy reload cũ nếu đang reload vũ khí cũ
        playerReload?.CancelReload();

        runtimeItem = newItem;

        // ✅ Gán lại các thành phần
        playerShooting?.SetWeapon(runtimeItem);
        playerReload?.SetWeapon(runtimeItem);
        ammoUI?.Bind(runtimeItem);
        modelViewer?.UpdateSprite(runtimeItem);

        Debug.Log($"[WEAPON EQUIP] Gán weapon: {newItem.baseData.itemName} | GUID: {newItem.guid} | Ammo: {newItem.currentAmmoType?.ammoName ?? "null"}");
    }
}
