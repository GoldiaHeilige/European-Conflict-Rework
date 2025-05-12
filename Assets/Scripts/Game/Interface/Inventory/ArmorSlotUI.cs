using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlotUI : InventorySlot
{
    public ArmorSlot armorSlotType; // Gán trong Inspector: Head, Body,...

    public override void OnDrop(PointerEventData eventData)
    {
/*        Debug.Log("OnDrop được gọi trong ArmorSlotUI");*/

        var dragSource = InventorySlotDragHandler.currentDraggingSlot;
        if (dragSource == null || dragSource == this)
        {
            Debug.LogWarning("Drag source null hoặc là chính slot");
            return;
        }

        var sourceItem = dragSource.GetItem();
        if (sourceItem == null || sourceItem.itemData == null)
        {
            Debug.LogWarning("Source item null hoặc thiếu itemData");
            return;
        }

        ArmorData armorData = sourceItem.itemData as ArmorData;
        if (armorData == null)
        {
/*            Debug.LogWarning("Không phải ArmorData");*/
            return;
        }

        if (armorData.armorSlot != armorSlotType)
        {
            Debug.LogWarning($"Slot không khớp: cần {armorSlotType}, item là {armorData.armorSlot}");
            return;
        }
        base.OnDrop(eventData);
    }

}
