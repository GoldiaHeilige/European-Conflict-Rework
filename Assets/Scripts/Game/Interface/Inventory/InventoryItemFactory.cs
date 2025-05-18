using UnityEngine;

public static class InventoryItemFactory
{
    public static InventoryItemRuntime Create(InventoryItemData itemData, int quantity = 1, bool log = true)
    {
        int? durability = null;

        if (itemData is ArmorData armor)
        {
            durability = armor.maxDurability;
        }

        if (itemData is WeaponData weaponData)
        {
            var ammo = PlayerInventory.Instance.GetDefaultAmmoFor(weaponData.weaponClass);
            var weaponItem = new WeaponRuntimeItem(weaponData, ammo);
            if (log)
                Debug.LogWarning($"🧨 WeaponRuntimeItem CREATED từ Factory — ID: {weaponItem.runtimeId} | Data: {itemData.itemID}");
            return weaponItem;
        }

        var newItem = new InventoryItemRuntime(itemData, quantity, durability);
        if (log)
            Debug.Log($"📦 InventoryItemRuntime CREATED từ Factory — ID: {newItem.runtimeId} | {itemData?.itemID}");
        return newItem;
    }
}

