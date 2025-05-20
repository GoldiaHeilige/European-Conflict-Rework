using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeaponRuntimeItem : InventoryItemRuntime
{
    public string guid;
    public WeaponData baseData;
    public AmmoData currentAmmoType;
    public int ammoInClip;

    public event Action OnAmmoChanged;

    public WeaponRuntimeItem(WeaponData baseData, AmmoData startingAmmo, string forcedId = null)
        : base(baseData, 1, null, forcedId) // truyền GUID từ prefab nếu có
    {
        this.baseData = baseData;
        this.guid = runtimeId; // đồng bộ lại (vì runtimeId nằm ở class cha)
        this.currentAmmoType = startingAmmo;
        this.ammoInClip = 0;
    }


    public bool CanFire() => ammoInClip > 0;
    public bool CanReload(PlayerInventory inventory)
    {
        // Không cho phép reload nếu bản vũ khí đã bị rút khỏi kho hoặc slot
        bool stillInInventory = inventory.weaponSlots.Any(w => w == this) ||
                                inventory.GetItems().Any(i => i == this);

        if (!stillInInventory)
        {
            Debug.LogWarning($"[CanReload] WeaponRuntimeItem {runtimeId} đã bị gỡ khỏi inventory nhưng vẫn đang được tham chiếu!");
            return false;
        }

        if (currentAmmoType == null || baseData == null)
        {
            Debug.LogWarning("[CanReload] currentAmmoType hoặc baseData null");
            return false;
        }

        var matched = inventory.knownAmmoTypes.FirstOrDefault(a => a.ammoName == currentAmmoType.ammoName);
        if (matched == null)
        {
            Debug.LogWarning("[CanReload] Ammo không nằm trong knownAmmoTypes");
            return false;
        }

        currentAmmoType = matched;

        int totalAmmo = inventory.GetAmmoCount(currentAmmoType);
        return ammoInClip < baseData.clipSize && totalAmmo > 0;
    }

    public void Reload(PlayerInventory inventory)
    {
        if (currentAmmoType == null)
        {
            Debug.LogError("[Reload] currentAmmoType null → bỏ qua");
            return;
        }

        // tìm đúng bản từ knownAmmoTypes để đảm bảo instance khớp
        var matched = inventory.knownAmmoTypes.FirstOrDefault(a => a.ammoName == currentAmmoType.ammoName);
        if (matched == null)
        {
            Debug.LogError($"[Reload] Không tìm thấy AmmoData '{currentAmmoType.ammoName}' trong knownAmmoTypes");
            return;
        }

        currentAmmoType = matched;

        int needed = baseData.clipSize - ammoInClip;
        int available = inventory.GetAmmoCount(currentAmmoType);

        if (available <= 0)
        {
            Debug.LogWarning($"[Reload] Không có đủ đạn để reload: {currentAmmoType.ammoName}");
            return;
        }

        int reloadAmount = Mathf.Min(needed, available);

        Debug.Log($"[Reload] Needed: {needed}, Available: {available}, ToLoad: {reloadAmount}, AmmoInClip: {ammoInClip}");

        ammoInClip += reloadAmount;
        inventory.RemoveAmmo(currentAmmoType, reloadAmount);

        OnAmmoChanged?.Invoke();
    }

    public void ConsumeBullet()
    {
        ammoInClip = Mathf.Max(ammoInClip - 1, 0);
        OnAmmoChanged?.Invoke();
    }

    public void ChangeAmmoType(AmmoData newAmmo, PlayerInventory inventory)
    {
        if (currentAmmoType != null)
            inventory.AddAmmo(currentAmmoType, ammoInClip);

        currentAmmoType = newAmmo;
        ammoInClip = Mathf.Min(baseData.clipSize, inventory.GetAmmoCount(newAmmo));
        inventory.RemoveAmmo(newAmmo, ammoInClip);

        OnAmmoChanged?.Invoke();
    }
}
