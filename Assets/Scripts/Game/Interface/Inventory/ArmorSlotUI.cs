using UnityEngine.EventSystems;
using UnityEngine;

public class ArmorSlotUI : InventorySlot
{
    public ArmorSlot armorSlotType;

    public override void OnDrop(PointerEventData eventData)
    {
        var dragSource = InventorySlotDragHandler.currentDraggingSlot;
        if (dragSource == null || dragSource == this)
            return;

        var draggedItem = dragSource.GetItem(); // bản trong inventory
        if (draggedItem == null || draggedItem.itemData == null)
            return;

        var armorData = draggedItem.itemData as ArmorData;
        if (armorData == null || armorData.armorSlot != armorSlotType)
        {
            Debug.Log("❌ Không thể thả item vào slot giáp sai loại.");
            return;
        }

        if (draggedItem.durability <= 0)
        {
            Debug.Log("❌ Không thể trang bị giáp đã vỡ.");
            return;
        }

        var player = GameObject.FindWithTag("Player");
        var armorManager = player?.GetComponent<EquippedArmorManager>();
        if (armorManager == null) return;

        var inv = PlayerInventory.Instance;

        // Xoá đúng bản gốc khỏi inventory
        for (int i = 0; i < inv.items.Count; i++)
        {
            if (inv.items[i] != null && inv.items[i].runtimeId == draggedItem.runtimeId)
            {
                inv.items[i] = null;
                PlayerInventory.Instance.RaiseInventoryChanged("Equip giap xong thi xoa slot");
                break;
            }
        }

        var currentEquipped = armorManager.GetArmor(armorSlotType);

        if (currentEquipped != null && currentEquipped.sourceItem.runtimeId != draggedItem.runtimeId)
        {
            inv.ReturnItemToInventory(currentEquipped.sourceItem);
        }

        // Trang bị bản gốc
        var armorRuntime = new ArmorRuntime(armorData, armorManager, draggedItem);
        Debug.Log($"[DROP] dùng lại bản gốc: {draggedItem.runtimeId}");
        Debug.Log($"[DEBUG] ArmorRuntime tạo xong: {armorRuntime.sourceItem.runtimeId}");
        armorManager.EquipArmor(armorRuntime, this);

        UpdateSlot(); // Cập nhật icon sau khi mặc

        base.OnDrop(eventData);
    }

    public override InventoryItemRuntime GetItem()
    {
        var player = GameObject.FindWithTag("Player");
        var armorManager = player?.GetComponent<EquippedArmorManager>();
        if (armorManager == null) return null;

        var armor = armorManager.GetArmor(armorSlotType);
        return armor?.sourceItem;
    }


    public void UpdateSlot()
    {
        var player = GameObject.FindWithTag("Player");
        var armorManager = player?.GetComponent<EquippedArmorManager>();
        if (armorManager == null) return;

        var armor = armorManager.GetArmor(armorSlotType);

        if (armor != null)
        {
            SetItem(armor.sourceItem); // icon từ item đã mặc
        }
        else
        {
            Clear(); // xoá icon nếu không có gì
        }
    }
}
