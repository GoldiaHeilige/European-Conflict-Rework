using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private GameObject iconHolderPrefab;
    private GameObject currentIconObj;

    public void SetItem(InventoryItemRuntime item)
    {
        Clear(); 
        if (item == null)
        {
            Debug.LogError("SetItem() nhận item NULL");
            return;
        }

        if (item.itemData == null)
        {
            Debug.LogError("itemData null!");
            return;
        }

        if (iconHolderPrefab == null)
        {
            Debug.LogError("iconHolderPrefab chưa được gán trong slot!");
            return;
        }

        currentIconObj = Instantiate(iconHolderPrefab, transform);

        RectTransform rt = currentIconObj.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogError("iconHolderPrefab KHÔNG có RectTransform!");
            return;
        }

        rt.SetParent(transform, false);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        Image img = currentIconObj.GetComponentInChildren<Image>();
        if (img == null)
        {
            Debug.LogError("❌ iconHolderPrefab thiếu Image con!");
            return;
        }

        img.sprite = item.itemData.icon;

        var text = currentIconObj.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = (item.itemData.stackable && item.quantity > 1)
                ? item.quantity.ToString()
                : "";
        }
    }


    public void Clear()
    {
        if (currentIconObj != null)
            Destroy(currentIconObj);
    }
}
