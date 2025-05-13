using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] private int maxSlot = 14;

    private List<InventoryItemRuntime> items = new();

    public delegate void OnInventoryChanged();

    public static event OnInventoryChanged InventoryChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<InventoryItemRuntime> GetItems()
    {
        List<InventoryItemRuntime> padded = new List<InventoryItemRuntime>(maxSlot);

        for (int i = 0; i < maxSlot; i++)
        {
            if (i < items.Count)
                padded.Add(items[i]);
            else
                padded.Add(null);
        }

        return padded;
    }


    public bool AddItem(InventoryItemData itemData, int amount = 1)
    {
        if (itemData == null)
        {
            Debug.LogError("AddItem: itemData bị NULL");
            return false;
        }

        int remaining = amount;

        if (itemData.stackable)
        {
            foreach (var item in items)
            {
                if (item != null && item.itemData == itemData && item.quantity < itemData.maxStack)
                {
                    int space = itemData.maxStack - item.quantity;
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
        if (itemData == null)
        {
            Debug.LogWarning("CanAddItem: itemData bị NULL");
            return false;
        }

        int remaining = amount;

        if (itemData.stackable)
        {
            foreach (var item in items)
            {
                if (item != null && item.itemData == itemData && item.quantity < itemData.maxStack)
                {
                    int space = itemData.maxStack - item.quantity;
                    int toAdd = Mathf.Min(space, remaining);
                    remaining -= toAdd;

                    if (remaining <= 0) return true;
                }
            }
        }

        int stackNeeded = Mathf.CeilToInt((float)remaining / itemData.maxStack);
        int availableSlot = maxSlot - items.FindAll(i => i != null).Count;

        return availableSlot >= stackNeeded;
    }

    public void SwapItemByIndex(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= maxSlot || indexB < 0 || indexB >= maxSlot)
        {
            Debug.LogWarning("SwapItemByIndex: Index nằm ngoài giới hạn logic!");
            return;
        }

        // Đảm bảo danh sách đủ slot
        while (items.Count < maxSlot)
            items.Add(null);

        var temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;

        InventoryChanged?.Invoke();
    }
}