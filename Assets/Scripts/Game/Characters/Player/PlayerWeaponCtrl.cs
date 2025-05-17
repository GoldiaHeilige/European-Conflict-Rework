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
            return;
        }

        runtimeItem = newItem;

        playerShooting?.SetWeapon(runtimeItem); // CHUYỂN TOÀN BỘ sang dùng WeaponRuntimeItem
        playerReload?.SetWeapon(runtimeItem);
        ammoUI?.Bind(runtimeItem);
        modelViewer?.UpdateSprite(runtimeItem);

        Debug.Log($"[WEAPON EQUIP] Đã gán weapon: {newItem.baseData.itemName}, GUID: {newItem.guid}");
    }
}
