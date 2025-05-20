
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int maxSlot = 14;
    public int maxSlotDisplay = 12;
    public WeaponRuntimeItem equippedWeapon;
    public PlayerModelViewer modelViewer;
    public static System.Action InventoryChanged;

    [HideInInspector]
    public List<InventoryItemRuntime> items = new();

    public WeaponRuntimeItem[] weaponSlots = new WeaponRuntimeItem[2]; // 0 = Primary, 1 = Secondary
    public int currentWeaponIndex = 0;

    public List<AmmoData> knownAmmoTypes;
    private Dictionary<AmmoData, int> ammoCounts = new();

    public void RaiseInventoryChanged(string reason)
    {
        Debug.Log($"[InventoryChanged] Triggered bởi: {reason}");
        InventoryChanged?.Invoke();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (items == null || items.Count == 0)
        {
            items = new List<InventoryItemRuntime>();
            EnsureSlotCount();
        }

        Debug.Log("[PlayerInventory] Awake hoàn tất");

        foreach (var ammo in knownAmmoTypes)
        {
            ammoCounts[ammo] = 0; // Init with 0
        }
    }

    public void EnsureSlotCount()
    {
        while (items.Count < maxSlot)
            items.Add(null);
    }

    public bool CanAddItem(InventoryItemData itemData, int amount)
    {
        EnsureSlotCount();

        if (itemData == null)
        {
            Debug.LogWarning("[CanAddItem] itemData null");
            return false;
        }

        Debug.Log($"[CanAddItem] Checking: {itemData.itemID} | stackable: {itemData.stackable}");

        if (itemData.stackable)
        {
            for (int i = 0; i < maxSlotDisplay; i++)
            {
                var slotItem = items[i];
                if (slotItem != null &&
                    slotItem.itemData != null &&
                    slotItem.itemData.itemID == itemData.itemID &&
                    slotItem.quantity < slotItem.itemData.maxStack)
                {
                    int space = slotItem.itemData.maxStack - slotItem.quantity;
                    Debug.Log($"[CanAddItem] Found stackable room in slot {i} (space: {space})");
                    if (space >= amount) return true;
                }
            }
        }

        for (int i = 0; i < maxSlotDisplay; i++)
        {
            var slotItem = items[i];
            string raw = slotItem == null ? "null" : (slotItem.itemData == null ? "itemData null" : slotItem.itemData.itemID);
            Debug.Log($"[CanAddItem] Slot {i} = {raw}");

            if (slotItem == null || slotItem.itemData == null)
            {
                Debug.Log($"[CanAddItem] Found empty slot {i}");
                return true;
            }
        }

        Debug.LogWarning("[CanAddItem] Inventory full");
        return false;
    }


    public bool AddStackableItem(InventoryItemData data, int quantity)
    {
        EnsureSlotCount();

        int remaining = quantity;

        // Ưu tiên cộng vào các stack đã có
        for (int i = 0; i < maxSlotDisplay && remaining > 0; i++)
        {
            var item = items[i];
            if (item != null && item.itemData == data && item.quantity < data.maxStack)
            {
                int canAdd = Mathf.Min(data.maxStack - item.quantity, remaining);
                item.quantity += canAdd;
                remaining -= canAdd;
            }
        }

        // Tạo stack mới nếu còn dư
        for (int i = 0; i < maxSlotDisplay && remaining > 0; i++)
        {
            if (items[i] == null || items[i].itemData == null)
            {
                int toCreate = Mathf.Min(data.maxStack, remaining);
                var newItem = InventoryItemFactory.Create(data, toCreate, log: true);
                items[i] = newItem;
                remaining -= toCreate;
            }
        }

        InventoryChanged?.Invoke();

        return remaining == 0;
    }



    public bool AddUniqueItem(InventoryItemRuntime newItem)
    {
        EnsureSlotCount();
        for (int i = 0; i < maxSlotDisplay; i++)
        {
            if (items[i] == null || items[i].itemData == null)
            {
                items[i] = newItem;
                InventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }



    public bool ReturnItemToInventory(InventoryItemRuntime item)
    {
        EnsureSlotCount();

        for (int i = 0; i < maxSlotDisplay; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                Debug.Log($"[ReturnItemToInventory] Gán lại bản gốc: {item.runtimeId}");
                InventoryChanged?.Invoke();
                break;
            }
        }

        if (item is WeaponRuntimeItem wpn)
        {
            // Check trong weaponSlots
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                var slot = weaponSlots[i];
                if (slot != null && slot.runtimeId == wpn.runtimeId)
                {
                    Debug.Log($"[ReturnItemToInventory] Weapon trong slot {i} bị trùng runtimeId. Xoá.");
                    weaponSlots[i] = null;
                }
            }

            // Check equippedWeapon riêng
            if (equippedWeapon != null && equippedWeapon.runtimeId == wpn.runtimeId)
            {
                Debug.Log($"[ReturnItemToInventory] Weapon đang cầm bị kéo về kho. Gỡ trang bị.");
                equippedWeapon = null;
                currentWeaponIndex = -1;
                PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
                modelViewer?.UpdateSprite(null);

                if (PlayerWeaponCtrl.Instance != null)
                {
                    PlayerWeaponCtrl.Instance.ClearWeapon();
                }
                else
                {
                    Debug.LogError("[PlayerInventory] PlayerWeaponCtrl.Instance == null → KHÔNG GỌI ĐƯỢC");
                }
            }
        }

        return true;
    }

    public void RemoveExactItem(InventoryItemRuntime target)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].runtimeId == target.runtimeId)
            {
                Debug.Log($"[RemoveExactItem] Xoá đúng item {target.runtimeId} ở slot {i}");
                items[i] = null;
                RaiseInventoryChanged("RemoveExactItem");
                return;
            }
        }
    }

    public AmmoData GetDefaultAmmoFor(WeaponClass weaponClass)
    {
        foreach (var ammo in knownAmmoTypes)
        {
            if (ammo.compatibleWeapon == weaponClass)
            {
                // Ưu tiên FMJ nếu có
                if (ammo.ammoName.ToLower().Contains("fmj"))
                    return ammo;
            }
        }

        // Không có FMJ → trả ammo bất kỳ phù hợp weaponClass
        Debug.LogWarning($"Không tìm thấy FMJ cho {weaponClass}, dùng bất kỳ đạn nào khớp weaponClass");
        return knownAmmoTypes.FirstOrDefault(a => a.compatibleWeapon == weaponClass);
    }


    public List<InventoryItemRuntime> GetItems()
    {
        return items;
    }

    public void EquipWeapon(WeaponRuntimeItem runtimeWeapon)
    {
        equippedWeapon = runtimeWeapon;
        PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
        modelViewer.UpdateSprite(runtimeWeapon);
        InventoryChanged?.Invoke();
    }


    public void UnequipWeapon()
    {
        equippedWeapon = null;
        modelViewer.UpdateSprite(null);
    }

    public void AssignWeaponToSlot(WeaponRuntimeItem weapon, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        weaponSlots[slotIndex] = weapon;
        PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
        InventoryChanged?.Invoke();
    }

    public void EquipWeaponSlot(int index)
    {
        if (index < 0 || index >= weaponSlots.Length)
        {
            Debug.LogWarning("[EquipWeaponSlot] Index không hợp lệ");
            return;
        }

        var weapon = weaponSlots[index];

        if (weapon == null || weapon.baseData == null)
        {
            equippedWeapon = null;
            currentWeaponIndex = -1;

            if (PlayerWeaponCtrl.Instance != null)
            {
                PlayerWeaponCtrl.Instance.ClearWeapon();
            }
            else
            {
                Debug.LogError("[PlayerInventory] PlayerWeaponCtrl.Instance == null → KHÔNG GỌI ĐƯỢC");
            }
            modelViewer?.UpdateSprite(null);
            return;
        }


        equippedWeapon = weapon;
        currentWeaponIndex = index;
        modelViewer?.UpdateSprite(equippedWeapon);

        PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
        Debug.Log($"[Inventory] Trang bị vũ khí {equippedWeapon.baseData.itemName} từ slot {index}.");
    }


    public void UnequipWeaponSlot(int index)
    {
        if (index < 0 || index >= weaponSlots.Length) return;

        var slotWeapon = weaponSlots[index];
        string slotGuid = slotWeapon?.guid;

        Debug.Log($"[DEBUG] UnequipWeaponSlot({index}) được gọi - slot GUID: {slotGuid}, equipped GUID: {equippedWeapon?.guid}");

        if (equippedWeapon != null && slotGuid != null && equippedWeapon.guid == slotGuid)
        {
            Debug.Log($"[UnequipWeaponSlot] Gỡ vũ khí đang trang bị từ slot {index} (GUID: {slotGuid})");

            equippedWeapon = null;
            currentWeaponIndex = -1;
            PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
            modelViewer?.UpdateSprite(null);

            if (PlayerWeaponCtrl.Instance != null)
            {
                PlayerWeaponCtrl.Instance.ClearWeapon();
            }
            else
            {
                Debug.LogError("[WeaponSlotUI] PlayerWeaponCtrl.Instance == null → KHÔNG GỌI ĐƯỢC");
            }
        }
        else
        {
            Debug.Log("[UnequipWeaponSlot] Không khớp GUID hoặc weapon null -> không huỷ trang bị");
        }

        weaponSlots[index] = null;
        InventoryChanged?.Invoke();
    }

    public void AddAmmo(AmmoData incomingAmmo, int amount)
    {
        var matched = knownAmmoTypes.FirstOrDefault(a => a.ammoName == incomingAmmo.ammoName);
        if (matched == null)
        {
            Debug.LogWarning($"[AddAmmo] Không tìm thấy ammoName: {incomingAmmo.ammoName}");
            return;
        }

        if (!ammoCounts.ContainsKey(matched))
            ammoCounts[matched] = 0;

        ammoCounts[matched] += amount;

        Debug.Log($"[AddAmmo] Thêm {amount} viên → {matched.ammoName} | Tổng: {ammoCounts[matched]}");

        // Nếu vũ khí đang cầm sử dụng đúng loại ammo này, ép đồng bộ lại luôn
        var weapon = equippedWeapon;
        if (weapon != null && weapon.currentAmmoType != null &&
            weapon.currentAmmoType.ammoName == matched.ammoName &&
            weapon.currentAmmoType != matched)
        {
            Debug.Log($"[AddAmmo] Đồng bộ lại ammo instance trên weapon đang cầm: {matched.ammoName}");
            weapon.currentAmmoType = matched;
        }

        PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
    }

    public bool RemoveAmmo(AmmoData ammo, int amount)
    {
        var matched = knownAmmoTypes.FirstOrDefault(a => a.ammoName == ammo.ammoName);
        if (matched == null || !ammoCounts.ContainsKey(matched) || ammoCounts[matched] < amount)
        {
            Debug.LogWarning($"[RemoveAmmo] Không thể trừ {amount} viên {ammo.ammoName} (matched = {matched?.ammoName ?? "null"})");
            return false;
        }

        ammoCounts[matched] -= amount;
        Debug.Log($"[RemoveAmmo] Trừ {amount} viên {matched.ammoName} | Còn lại: {ammoCounts[matched]}");

        int remainingToRemove = amount;

        foreach (var item in items)
        {
            if (item is InventoryItemRuntime ammoItem &&
                ammoItem.itemData is AmmoItemData ammoItemData &&
                ammoItemData.linkedAmmoData.ammoName == matched.ammoName)
            {
                int removeFromThis = Mathf.Min(ammoItem.quantity, remainingToRemove);
                ammoItem.quantity -= removeFromThis;
                remainingToRemove -= removeFromThis;

                if (remainingToRemove <= 0)
                    break;
            }
        }

        PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
        InventoryChanged?.Invoke();
        return true;
    }

    /*    public int GetAmmoCount(AmmoData ammo) => ammoCounts.TryGetValue(ammo, out var count) ? count : 0;*/

    public int GetAmmoCount(AmmoData ammo)
    {
        var matched = knownAmmoTypes.FirstOrDefault(a => a.ammoName == ammo.ammoName);
        return matched != null && ammoCounts.TryGetValue(matched, out int count) ? count : 0;
    }

    public void RemoveByGUID(string guid)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].runtimeId == guid)
            {
                Debug.Log($"[RemoveByGUID] Xoá item {guid} tại slot {i}");
                items[i] = null;
                RaiseInventoryChanged("RemoveByGUID");
                return;
            }
        }

        Debug.LogWarning($"[RemoveByGUID] Không tìm thấy item với GUID: {guid}");
    }

    public void DestroyByGUID(string guid)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].runtimeId == guid)
            {
                Debug.Log($"[DestroyByGUID] Item {guid} bị huỷ tại slot {i}");
                items[i] = null;
                RaiseInventoryChanged("DestroyByGUID");
                return;
            }
        }

        Debug.LogWarning($"[DestroyByGUID] Không tìm thấy item để huỷ với GUID: {guid}");
    }



    public void PrintInventoryDebug(string prefix = "")
    {
        Debug.Log($"[DEBUG] --- {prefix} INVENTORY ---");

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item != null)
            {
                Debug.Log($"[INV] Slot {i} | ID: {item.runtimeId} | {item.itemData?.itemID}");
            }
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            var item = weaponSlots[i];
            if (item != null)
            {
                Debug.Log($"[WPN] Slot {i} | ID: {item.runtimeId} | {item.baseData?.itemID}");
            }
        }

        if (equippedWeapon != null)
        {
            Debug.Log($"[EQUIPPED] {equippedWeapon.runtimeId} | {equippedWeapon.baseData?.itemID}");
        }
    }

}
