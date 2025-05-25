
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

    public float SoftLimit = 35f;
    public float HardLimit = 42f;
    public float MaxWeight = 50f;

    public float equippedWeightReductionFactor = 0.5f; // 50% nếu mặc
    public float wieldedWeaponWeightFactor = 0.75f;     // 75% nếu đang cầm

    public float TotalWeight
    {
        get
        {
            float total = 0f;

            // Kho đồ thường
            total += items.Where(i => i != null).Sum(i => i.TotalWeight);

            // Vũ khí trang bị
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                var wpn = weaponSlots[i];
                if (wpn == null) continue;

                float factor = (wpn == equippedWeapon) ? wieldedWeaponWeightFactor : 1f;
                total += wpn.TotalWeight * factor;
            }

            // Giáp đang mặc
            var armorMgr = GetComponent<EquippedArmorManager>();
            if (armorMgr != null)
            {
                foreach (var armor in armorMgr.GetAllEquippedArmors())
                {
                    if (armor?.sourceItem != null)
                    {
                        total += armor.sourceItem.TotalWeight * equippedWeightReductionFactor;
                    }
                }
            }

            return total;
        }
    }

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


    public bool AddStackableItem(InventoryItemRuntime item)
    {
        EnsureSlotCount();
        int remaining = item.quantity;

        // Ưu tiên cộng vào các stack đã có
        for (int i = 0; i < maxSlotDisplay && remaining > 0; i++)
        {
            var slotItem = items[i];
            if (slotItem != null &&
                slotItem.itemData == item.itemData &&
                slotItem.quantity < item.itemData.maxStack)
            {
                int canAdd = Mathf.Min(item.itemData.maxStack - slotItem.quantity, remaining);
                slotItem.quantity += canAdd;
                remaining -= canAdd;
            }
        }

        // Nếu còn dư → giữ lại GUID gốc và thêm vào slot trống
        if (remaining > 0)
        {
            for (int i = 0; i < maxSlotDisplay; i++)
            {
                if (items[i] == null)
                {
                    var clone = new InventoryItemRuntime(item.itemData, remaining, null, item.runtimeId);
                    items[i] = clone;
                    remaining = 0;
                    break;
                }
            }
        }

        RaiseInventoryChanged("AddStackableItem(runtime)");
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
        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
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
        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
        modelViewer.UpdateSprite(runtimeWeapon);
        RaiseInventoryChanged("EquipWeapon weight update");
        InventoryChanged?.Invoke();
    }


    public void UnequipWeapon()
    {
        if (equippedWeapon != null)
        {
            string targetGuid = equippedWeapon.guid;

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] != null && weaponSlots[i].guid == targetGuid)
                {
                    Debug.Log($"[UnequipWeapon] Gỡ khỏi weaponSlots[{i}] do match GUID");
                    weaponSlots[i] = null;
                    break;
                }
            }
        }

        equippedWeapon = null;
        currentWeaponIndex = -1;
        modelViewer.UpdateSprite(null);
        PlayerWeaponCtrl.Instance?.ClearWeapon();
        RaiseInventoryChanged("UnequipWeapon");
    }


    public void AssignWeaponToSlot(WeaponRuntimeItem weapon, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        weaponSlots[slotIndex] = weapon;
        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
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

        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
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
    PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
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
        if (incomingAmmo == null)
        {
            Debug.LogWarning("[AddAmmo] incomingAmmo null → bỏ qua");
            return;
        }

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

        var weapon = equippedWeapon;
        if (weapon != null)
        {
            if (weapon.currentAmmoType != null &&
                weapon.currentAmmoType.ammoName == matched.ammoName &&
                weapon.currentAmmoType != matched)
            {
                Debug.Log($"[AddAmmo] Đồng bộ lại ammo instance trên weapon đang cầm: {matched.ammoName}");
                weapon.currentAmmoType = matched;
            }

            weapon.CheckAmmoValid();
        }

    PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
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

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] is InventoryItemRuntime ammoItem &&
                ammoItem.itemData is AmmoItemData ammoItemData &&
                ammoItemData.linkedAmmoData.ammoName == matched.ammoName)
            {
                int removeFromThis = Mathf.Min(ammoItem.quantity, remainingToRemove);
                ammoItem.quantity -= removeFromThis;
                remainingToRemove -= removeFromThis;

                // 🧨 Nếu về 0 → xoá hẳn khỏi slot
                if (ammoItem.quantity <= 0)
                {
                    Debug.Log($"[RemoveAmmo] Slot {i} rỗng sau khi trừ → xoá");
                    items[i] = null;
                }

                if (remainingToRemove <= 0)
                    break;
            }
        }

        // ✅ Sửa lại bằng so sánh GetInstanceID()
        if (equippedWeapon != null &&
            equippedWeapon.currentAmmoType != null &&
            equippedWeapon.currentAmmoType.ammoName == matched.ammoName &&
            equippedWeapon.currentAmmoType.GetInstanceID() != matched.GetInstanceID())
        {
            Debug.LogWarning($"[RemoveAmmo] AmmoData mismatch → cập nhật lại ammoType trên weapon!");
            Debug.Log($"[Ammo DEBUG] Weapon đang dùng instanceID: {equippedWeapon.currentAmmoType.GetInstanceID()}");
            Debug.Log($"[Ammo DEBUG] matched instanceID: {matched.GetInstanceID()}");

            equippedWeapon.currentAmmoType = matched;
        }

        equippedWeapon?.CheckAmmoValid();
        RaiseInventoryChanged("RemoveAmmo");
        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
        return true;
    }


    public void ForceSetAmmoCount(AmmoData ammo, int count)
    {
        var matched = knownAmmoTypes.FirstOrDefault(a => a.ammoName == ammo.ammoName);
        if (matched == null) return;

        ammoCounts[matched] = Mathf.Max(0, count);
        Debug.Log($"[ForceSetAmmoCount] {ammo.ammoName} → {count}");
    }



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
            var item = items[i];
            if (item != null && item.runtimeId == guid)
            {
                Debug.Log($"[DestroyByGUID] Item {guid} bị huỷ tại slot {i}");

                RemoveFromSpecificStack(item, item.quantity); // ✅ trừ đúng stack và ammoCounts
                return;
            }
        }

        Debug.LogWarning($"[DestroyByGUID] Không tìm thấy item để huỷ với GUID: {guid}");
    }


    public bool RemoveFromSpecificStack(InventoryItemRuntime targetStack, int amount)
    {
        if (targetStack == null || amount <= 0) return false;

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item != null && item.runtimeId == targetStack.runtimeId)
            {
                int toRemove = Mathf.Min(item.quantity, amount);
                item.quantity -= toRemove;

                Debug.Log($"[RemoveFromSpecificStack] Trừ {toRemove} từ stack {item.runtimeId} tại slot {i}");

                // ✅ Trừ ammoCounts nếu là AmmoItemData
                if (item.itemData is AmmoItemData ammoItem)
                {
                    var matched = knownAmmoTypes.FirstOrDefault(a => a.ammoName == ammoItem.linkedAmmoData.ammoName);
                    if (matched != null)
                    {
                        if (!ammoCounts.ContainsKey(matched)) ammoCounts[matched] = 0;
                        ammoCounts[matched] = Mathf.Max(0, ammoCounts[matched] - toRemove);
                        Debug.Log($"[RemoveFromSpecificStack] ammoCounts[{matched.ammoName}] còn lại: {ammoCounts[matched]}");
                    }
                }

                if (item.quantity <= 0)
                {
                    Debug.Log($"[RemoveFromSpecificStack] Stack {item.runtimeId} về 0 → xoá slot");
                    items[i] = null;
                }

                RaiseInventoryChanged("RemoveFromSpecificStack");
        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
                return true;
            }
        }

        Debug.LogWarning("[RemoveFromSpecificStack] Không tìm thấy stack cần trừ");
        return false;
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

    public void AddItem(InventoryItemRuntime item)
    {
        if (item == null || item.itemData == null)
        {
            Debug.LogWarning("[AddItem] Item null hoặc không có itemData");
            return;
        }

        if (item.itemData.stackable)
        {
            PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
            AddStackableItem(item);
        }
        else
        {
            AddUniqueItem(item);
        }
    }

    public List<AmmoItemData> GetAllAmmoItems()
    {
        return items
            .Where(i => i != null && i.itemData is AmmoItemData)
            .Select(i => i.itemData as AmmoItemData)
            .Distinct()
            .ToList();
    }

}
