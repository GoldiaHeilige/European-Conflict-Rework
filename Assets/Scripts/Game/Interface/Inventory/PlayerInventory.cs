
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [HideInInspector]
    public List<InventoryItemRuntime> items = new();
    public int maxSlot = 14;
    public int maxSlotDisplay = 12;
    public static System.Action InventoryChanged;

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


    public bool AddStackableItem(InventoryItemData itemData, int amount)
    {
        EnsureSlotCount();
        int remaining = amount;

        for (int i = 0; i < maxSlotDisplay; i++)
        {
            var slotItem = items[i];
            if (slotItem != null && slotItem.itemData != null && slotItem.itemData.itemID == itemData.itemID && slotItem.quantity < slotItem.itemData.maxStack)
            {
                int space = slotItem.itemData.maxStack - slotItem.quantity;
                int toAdd = Mathf.Min(space, remaining);
                slotItem.quantity += toAdd;
                remaining -= toAdd;
                Debug.Log($"[InventoryChanged] Triggered từ AddStackableItem 1 | Slot 0 = {items[0]?.itemData?.itemID ?? "null"}");
                InventoryChanged?.Invoke();
                if (remaining <= 0) return true;
            }
        }

        for (int i = 0; i < maxSlotDisplay; i++)
        {
            if (items[i] == null)
            {
                int toAdd = Mathf.Min(itemData.maxStack, remaining);
                items[i] = InventoryItemFactory.Create(itemData, toAdd);
                remaining -= toAdd;
                Debug.Log($"[InventoryChanged] Triggered từ AddStackableItem 2 | Slot 0 = {items[0]?.itemData?.itemID ?? "null"}");
                InventoryChanged?.Invoke();
                if (remaining <= 0) return true;
            }
        }

        return false;
    }

    public bool AddUniqueItem(InventoryItemRuntime newItem)
    {
        EnsureSlotCount();
        for (int i = 0; i < maxSlotDisplay; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem; // ❌ Đừng clone ở đây nữa
                Debug.Log($"[AddUniqueItem] Added item: {newItem.itemData?.itemID} | ID: {newItem.runtimeId}");
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

    public List<InventoryItemRuntime> GetItems()
    {
        return items;
    }
}
