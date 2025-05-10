using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private List<InventoryItemRuntime> items = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<InventoryItemRuntime> GetItems() => items;

    public void AddItem(InventoryItemData itemData, int amount = 1)
    {
        if (itemData.stackable)
        {
            var found = items.Find(i => i.itemData == itemData);
            if (found != null)
            {
                found.quantity = Mathf.Min(found.quantity + amount, itemData.maxStack);
                return;
            }
        }

        items.Add(new InventoryItemRuntime(itemData, amount));
    }
}
