using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject iconHolderPrefab;
    private GameObject currentIconObj;

    private InventoryItemRuntime currentItem;

    public void SetItem(InventoryItemRuntime item)
    {
        Clear();

        this.currentItem = item;

        if (item == null)
        {
            Debug.LogWarning("SetItem() nhận item NULL");
            return;
        }

        if (item.itemData == null)
        {
            Debug.LogWarning("itemData null!");
            return;
        }

        if (iconHolderPrefab == null)
        {
            Debug.LogWarning("iconHolderPrefab chưa được gán trong slot!");
            return;
        }

        currentIconObj = Instantiate(iconHolderPrefab, transform);

        RectTransform rt = currentIconObj.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogWarning("iconHolderPrefab KHÔNG có RectTransform!");
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

        /*if (this is ArmorSlotUI)
        {
            rt.localScale = new Vector3(1.2f, 1.2f, 1);
        }*/

        if (this is ArmorSlotUI armorSlot)
        {
            if (armorSlot.armorSlotType == ArmorSlot.Head)
                rt.localScale = new Vector3(1.8f, 1.8f, 1);
            else
                rt.localScale = new Vector3(1.5f, 1.5f, 1);
        }


        Image img = currentIconObj.GetComponentInChildren<Image>();
        if (img == null)
        {
            Debug.LogWarning("iconHolderPrefab thiếu Image con!");
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
        currentItem = null;
        if (currentIconObj != null)
            Destroy(currentIconObj);
    }

    public InventoryItemRuntime GetItem()
    {
        return currentItem;
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        var dragSource = InventorySlotDragHandler.currentDraggingSlot;

        if (dragSource == null || dragSource == this)
            return;

        var sourceItem = dragSource.GetItem();
        var targetItem = this.GetItem();

        Debug.Log($"DROP: Source = {sourceItem?.itemData.itemName}, Target = {(targetItem == null ? "EMPTY" : targetItem.itemData.itemName)}");

        if (targetItem == null)
        {
            this.SetItem(sourceItem);
            dragSource.Clear();
            return;
        }

        // swap
        dragSource.SetItem(targetItem);
        this.SetItem(sourceItem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventorySlotDragHandler.currentDraggingSlot == null) return;

        var draggingItem = InventorySlotDragHandler.currentDraggingSlot.GetItem();
        if (draggingItem == null) return;

        bool isSwap = HasItem();
        SlotHighlightPreview.Instance.Show(draggingItem.itemData.icon, isSwap);
        SlotHighlightPreview.Instance.MoveTo(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SlotHighlightPreview.Instance.Hide();
    }
}
