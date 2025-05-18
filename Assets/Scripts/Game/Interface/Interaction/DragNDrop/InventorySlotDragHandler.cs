using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public static InventorySlot currentDraggingSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var parentSlot = GetComponentInParent<InventorySlot>();

        // Thêm check null để tránh crash
        if (parentSlot == null || !parentSlot.HasItem() || parentSlot.GetItem() == null)
        {
            Debug.LogWarning("[OnBeginDrag] Slot rỗng hoặc item null → hủy drag.");
            return;
        }

        currentDraggingSlot = parentSlot;
        DragPreviewSystem.Instance.Show(parentSlot.GetItem().itemData.icon);
        canvasGroup.blocksRaycasts = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        DragPreviewSystem.Instance.MoveTo(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        DragPreviewSystem.Instance.Hide();
        currentDraggingSlot = null;
    }
}
