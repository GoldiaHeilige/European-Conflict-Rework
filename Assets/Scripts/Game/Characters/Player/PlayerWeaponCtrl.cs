
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

        playerShooting?.SetWeapon(runtimeItem);
        playerReload?.SetWeapon(runtimeItem);
        ammoUI?.Bind(runtimeItem);
        modelViewer?.UpdateSprite(runtimeItem);

        Debug.Log($"[WEAPON EQUIP] Gán weapon: {newItem.baseData.itemName} | GUID: {newItem.guid}");
        Debug.Log($"[EquipWeapon] WeaponCtrl ID: {GetInstanceID()} | gán = {newItem?.runtimeId ?? "null"}");
    }

    public void ClearWeapon()
    {
/*        Debug.LogWarning($"[ClearWeapon] GỌI! Trên PlayerWeaponCtrl ID = {GetInstanceID()}");*/

        runtimeItem = null;
        playerShooting?.SetWeapon(null);
        playerReload?.SetWeapon(null);
        ammoUI?.Bind(null);
        modelViewer?.UpdateSprite(null);
    }
}
