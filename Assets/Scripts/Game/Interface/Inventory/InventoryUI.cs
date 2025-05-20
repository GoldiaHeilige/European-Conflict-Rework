using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [Header("Stat Display")]
    public TextMeshProUGUI hpText;

    [Header("Slot Holder")]
    public Transform slotParent;
    public InventorySlot[] slots;


    private void Awake()
    {
        Debug.Log("InventoryUI Awake");

        if (PlayerInventory.Instance == null)
        {
            Debug.LogWarning("PlayerInventory.Instance vẫn null khi InventoryUI Awake");
            // Delay Refresh bằng coroutine
            StartCoroutine(WaitForInventory());
            return;
        }

        RefreshUI();
    }

    private IEnumerator WaitForInventory()
    {
        yield return new WaitUntil(() => PlayerInventory.Instance != null);
        Debug.Log("[InventoryUI] WaitUntil thành công → gọi RefreshUI");
        RefreshUI();
    }

    private void OnEnable()
    {
        PlayerInventory.InventoryChanged -= RefreshUI; // tránh double
        PlayerInventory.InventoryChanged += RefreshUI;

        // Gọi lại luôn nếu Instance đã có
        if (PlayerInventory.Instance != null)
            RefreshUI();
    }


    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        var items = PlayerInventory.Instance.GetItems();
/*        Debug.Log($"[RefreshUI] gọi lại – Slot 0: {items[0]?.itemData?.itemID ?? "null"} | ID: {items[0]?.runtimeId ?? "null"}");*/
        /*        Debug.Log($"[UpdateInventory] Gọi lại – Count: {items?.Count}");*/
        UpdateInventory(items);
    }


    public void UpdateStats(float hp)
    {
        hpText.text = $"{hp}";
    }

    public void UpdateInventory(List<InventoryItemRuntime> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
/*            Debug.Log($"[UpdateInventory] Gọi lại – Count: {items?.Count}");*/

            if (slots[i] == null)
            {
                /*                Debug.Log($"[UpdateInventory] Slot 0: {items[0]?.itemData?.itemID ?? "null"}");*/
                Debug.LogError($"Slot {i} bị null!");
                continue;
            }

            slots[i].slotIndex = i;

            slots[i].slotIndex = i;

            var item = (i < items.Count) ? items[i] : null;
            string itemID = item?.itemData?.itemID ?? "null";
            string runtimeID = item?.runtimeId ?? "null";

            /*            Debug.Log($"[UI] Slot {i} | item: {itemID} | ID: {runtimeID}");*/

            if (item != null && item.itemData != null)
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
