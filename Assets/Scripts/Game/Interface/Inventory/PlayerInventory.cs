using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private List<InventoryItemRuntime> items = new();

    public delegate void OnInventoryChanged();

    public static event OnInventoryChanged InventoryChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<InventoryItemRuntime> GetItems() => items;

    public bool AddItem(InventoryItemData itemData, int amount = 1)
    {
        int remaining = amount;

        if (itemData.stackable)
        {
            // 1. Ưu tiên nhét vào stack đã có
            foreach (var item in items)
            {
                if (item.itemData == itemData && item.quantity < itemData.maxStack)
                {
                    int space = itemData.maxStack - item.quantity;
                    int toAdd = Mathf.Min(space, remaining);
                    item.quantity += toAdd;
                    remaining -= toAdd;

                    if (remaining <= 0) break;
                }
            }
        }

        // 2. Thêm stack mới nếu vẫn còn dư
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
        int remaining = amount;

        if (itemData.stackable)
        {
            foreach (var item in items)
            {
                if (item.itemData == itemData && item.quantity < itemData.maxStack)
                {
                    int space = itemData.maxStack - item.quantity;
                    int toAdd = Mathf.Min(space, remaining);
                    remaining -= toAdd;

                    if (remaining <= 0) return true;
                }
            }
        }

        int stackNeeded = Mathf.CeilToInt((float)remaining / itemData.maxStack);
        int maxSlot = 12;
        int availableSlot = maxSlot - items.Count;

        return availableSlot >= stackNeeded;
    }

}