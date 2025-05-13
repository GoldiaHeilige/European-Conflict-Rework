
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlotUI : InventorySlot
{
    public ArmorSlot armorSlotType;

    public override void OnDrop(PointerEventData eventData)
    {
        var dragSource = InventorySlotDragHandler.currentDraggingSlot;
        if (dragSource == null || dragSource == this)
            return;

        var runtimeItem = dragSource.GetItem();
        if (runtimeItem == null || runtimeItem.itemData == null)
            return;

        var armorData = runtimeItem.itemData as ArmorData;
        if (armorData == null || armorData.armorSlot != armorSlotType)
            return;

        if (runtimeItem.durability <= 0)
        {
            Debug.Log("❌ Không thể trang bị giáp đã vỡ.");
            return;
        }

        var player = GameObject.FindWithTag("Player");
        var armorManager = player?.GetComponent<EquippedArmorManager>();

        if (armorManager != null)
        {
            var armorRuntime = new ArmorRuntime(armorData, armorManager);
            armorRuntime.currentDurability = runtimeItem.durability;
            armorManager.EquipArmor(armorRuntime);
        }

        base.OnDrop(eventData);
    }
}
