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
            Debug.Log("Không thể thả item vào slot giáp sai loại.");
            return;
        }

        if (draggedItem.durability <= 0)
        {
            Debug.Log("Không thể trang bị giáp đã vỡ.");
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
                PlayerInventory.Instance.RaiseInventoryChanged("");
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
/*        Debug.Log($"[DROP] dùng lại bản gốc: {draggedItem.runtimeId}");
        Debug.Log($"[DEBUG] ArmorRuntime tạo xong: {armorRuntime.sourceItem.runtimeId}");*/
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
        if (armorManager == null)
        {
            Debug.LogError("[ArmorSlotUI] Không tìm thấy EquippedArmorManager");
            return;
        }

        var armor = armorManager.GetArmor(armorSlotType);

/*        Debug.Log($"[UpdateSlot] Slot {armorSlotType} đang {(armor != null ? "CÓ" : "KHÔNG")} giáp");*/

        if (armor != null)
        {
            SetItem(armor.sourceItem);
        }
        else
        {
            Clear();
        }
    }


    private void OnEnable()
    {
        Debug.Log("[ArmorSlotUI] Đăng ký InventoryChanged");
        PlayerInventory.InventoryChanged -= UpdateSlot; // tránh double
        PlayerInventory.InventoryChanged += UpdateSlot;
    }


    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= UpdateSlot;
    }

}
