using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Stat Display")]
    public TextMeshProUGUI hpText;

    [Header("Slot Holder")]
    public Transform slotParent;
    public InventorySlot[] slots;

    public void UpdateStats(float hp)
    {
        hpText.text = $"{hp}";
    }

    public void UpdateInventory(List<InventoryItemRuntime> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count) slots[i].SetItem(items[i]);
            else slots[i].Clear();
        }
    }
}
