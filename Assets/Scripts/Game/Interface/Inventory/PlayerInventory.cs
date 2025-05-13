
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public List<InventoryItemRuntime> items = new();
    public int maxSlot = 14;
    public static System.Action InventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("⚠️ Nhiều hơn một PlayerInventory trong scene!");
        }
    }

    public bool AddItem(InventoryItemData itemData, int amount = 1)
    {
        if (itemData == null) return false;

        int remaining = amount;

        if (itemData.stackable)
        {
            foreach (var item in items)
            {
                if (item != null && item.itemData == itemData && item.quantity < item.itemData.maxStack)
                {
                    int space = item.itemData.maxStack - item.quantity;
                    int toAdd = Mathf.Min(space, remaining);
                    item.quantity += toAdd;
                    remaining -= toAdd;
                    if (remaining <= 0) break;
                }
            }
        }

        while (remaining > 0)
        {
            int toAdd = Mathf.Min(itemData.maxStack, remaining);
            items.Add(new InventoryItemRuntime(itemData, toAdd));
            remaining -= toAdd;
        }

        InventoryChanged?.Invoke();
        return true;
    }

    public bool CanAddItem(InventoryItemData itemData, int amount)
    {
        if (itemData == null) return false;

        int remaining = amount;

        if (itemData.stackable)
        {
            foreach (var item in items)
            {
                if (item != null && item.itemData == itemData && item.quantity < item.itemData.maxStack)
                {
                    int space = item.itemData.maxStack - item.quantity;
                    int toAdd = Mathf.Min(space, remaining);
                    remaining -= toAdd;
                    if (remaining <= 0) return true;
                }
            }
        }

        int stackNeeded = Mathf.CeilToInt((float)remaining / itemData.maxStack);
        int availableSlot = maxSlot - items.Count(item => item != null);
        return availableSlot >= stackNeeded;
    }

    public List<InventoryItemRuntime> GetItems()
    {
        List<InventoryItemRuntime> result = new();
        for (int i = 0; i < maxSlot; i++)
        {
            if (i < items.Count) result.Add(items[i]);
            else result.Add(null);
        }
        return result;
    }

    public void RemoveItemByReference(InventoryItemData data)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].itemData == data)
            {
                items[i] = null;
                InventoryChanged?.Invoke();
                Debug.Log($"🗑️ Giáp {data.name} đã bị xoá khỏi inventory");
                break;
            }
        }
    }

    public void UpdateDurability(InventoryItemData data, int durability)
    {
        foreach (var item in items)
        {
            if (item != null && item.itemData == data)
            {
                item.durability = durability;
                Debug.Log($"🔄 Cập nhật durability giáp {data.name} về {durability}");
                break;
            }
        }
    }

    public void SwapItemByIndex(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= maxSlot || indexB < 0 || indexB >= maxSlot)
        {
            Debug.LogWarning("SwapItemByIndex: Index nằm ngoài giới hạn!");
            return;
        }

        while (items.Count < maxSlot)
            items.Add(null);

        var temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;

        InventoryChanged?.Invoke();
    }
}
