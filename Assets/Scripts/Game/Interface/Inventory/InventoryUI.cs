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

    private void OnEnable()
    {
        PlayerInventory.InventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        UpdateInventory(PlayerInventory.Instance.GetItems());
    }


    public void UpdateStats(float hp)
    {
        hpText.text = $"{hp}";
    }

    public void UpdateInventory(List<InventoryItemRuntime> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogError($"Slot {i} bị null!");
                continue;
            }

            slots[i].slotIndex = i;

            if (i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

}
