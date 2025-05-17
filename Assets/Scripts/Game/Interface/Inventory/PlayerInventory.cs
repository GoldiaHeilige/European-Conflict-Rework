
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        // Ưu tiên stack vào item đã có
        for (int i = 0; i < maxSlotDisplay; i++)
        {
            if (items[i] != null && items[i].itemData == data)
            {
                items[i].quantity += quantity;
                InventoryChanged?.Invoke();
                return true;
            }
        }

        // Nếu không stack được thì tìm slot trống
        for (int i = 0; i < maxSlotDisplay; i++)
        {
            if (items[i] == null || items[i].itemData == null)
            {
                var newItem = InventoryItemFactory.Create(data, quantity);
                items[i] = newItem;
                InventoryChanged?.Invoke();
                return true;
            }
        }

        return false;
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
                items[i] = item; // ✅ không còn clone nữa
                Debug.Log($"[ReturnItemToInventory] Gán lại bản gốc: {item.runtimeId}");
                InventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public void RemoveExactItem(InventoryItemRuntime target)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == target)
            {
                items[i] = null;
                Debug.Log($"[InventoryChanged] Triggered từ RemoveExactItem | Slot 0 = {items[0]?.itemData?.itemID ?? "null"}");
                InventoryChanged?.Invoke();
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
        modelViewer.UpdateSprite(runtimeWeapon);
        InventoryChanged?.Invoke(); 
    }


    public void UnequipWeapon()
    {
        equippedWeapon = null;
        modelViewer.UpdateSprite(null);
    }

    public void AddAmmo(AmmoData incomingAmmo, int amount)
    {
        var matchedAmmo = knownAmmoTypes.FirstOrDefault(a => a.ammoName == incomingAmmo.ammoName);
        if (matchedAmmo == null)
        {
            Debug.LogWarning($"[AddAmmo] Không tìm thấy ammoName: {incomingAmmo.ammoName}");
            return;
        }

        if (!ammoCounts.ContainsKey(matchedAmmo)) ammoCounts[matchedAmmo] = 0;
        ammoCounts[matchedAmmo] += amount;
    }

    public void AssignWeaponToSlot(WeaponRuntimeItem weapon, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        weaponSlots[slotIndex] = weapon;
        InventoryChanged?.Invoke();
    }
    public void EquipWeaponSlot(int index)
    {
        if (index < 0 || index >= weaponSlots.Length) return;

        var weapon = weaponSlots[index];
        if (weapon == null || weapon.baseData == null) return;

        equippedWeapon = weapon;
        currentWeaponIndex = index; // cập nhật index đang cầm
        weaponSlots[index] = null;

        modelViewer.UpdateSprite(equippedWeapon);
        Debug.Log($"[Inventory] Trang bị vũ khí {equippedWeapon.baseData.itemName} từ slot {index}.");
    }



    public void UnequipWeaponSlot(int index)
    {
        if (index < 0 || index >= weaponSlots.Length) return;

        var unequippedItem = weaponSlots[index];
        if (unequippedItem != null)
        {
            // Nếu đang cầm vũ khí này thì chuyển về tay không
            if (equippedWeapon == unequippedItem)
            {
                equippedWeapon = null;
                modelViewer.UpdateSprite(null);
                currentWeaponIndex = -1;
                Debug.Log($"[UnequipWeaponSlot] Đã gỡ vũ khí đang trang bị từ slot {index}, chuyển về tay không.");
            }

            weaponSlots[index] = null;
            InventoryChanged?.Invoke();
        }
    }



    public bool RemoveAmmo(AmmoData ammo, int amount)
    {
        if (!ammoCounts.ContainsKey(ammo) || ammoCounts[ammo] < amount) return false;
        ammoCounts[ammo] -= amount;
        return true;
    }

    /*    public int GetAmmoCount(AmmoData ammo) => ammoCounts.TryGetValue(ammo, out var count) ? count : 0;*/

    public int GetAmmoCount(AmmoData ammo)
    {
        var matched = knownAmmoTypes.FirstOrDefault(a => a.ammoName == ammo.ammoName);
        return matched != null && ammoCounts.TryGetValue(matched, out int count) ? count : 0;

    }
}
