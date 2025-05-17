using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotUI : InventorySlot
{
    public WeaponSlotType slotType;

    public new int slotIndex => slotType == WeaponSlotType.Primary ? 14 : 15;

    private int GetWeaponArrayIndex()
    {
        return slotType == WeaponSlotType.Primary ? 0 : 1;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var dragSource = InventorySlotDragHandler.currentDraggingSlot;
        if (dragSource == null || dragSource == this)
            return;

        var draggedItem = dragSource.GetItem();
        if (draggedItem == null || !(draggedItem.itemData is WeaponData))
        {
            Debug.LogWarning("[WeaponSlotUI] Item không hợp lệ hoặc không phải WeaponData.");
            return;
        }

        if (draggedItem is not WeaponRuntimeItem runtime)
        {
            Debug.LogWarning("[WeaponSlotUI] Item không phải WeaponRuntimeItem.");
            return;
        }


        int arrayIndex = GetWeaponArrayIndex();
        Debug.Log($"[WeaponSlotUI] SlotType={slotType}, ArrayIndex={arrayIndex}, Length={PlayerInventory.Instance.weaponSlots.Length}");
        var inv = PlayerInventory.Instance;

        // Gỡ khỏi inventory
        for (int i = 0; i < inv.items.Count; i++)
        {
            if (inv.items[i] != null && inv.items[i].runtimeId == runtime.runtimeId)
            {
                inv.items[i] = null;
                inv.RaiseInventoryChanged("Equip weapon xong thì xóa khỏi kho");
                break;
            }
        }

        var current = inv.weaponSlots[arrayIndex];
        if (current != null && current.runtimeId != runtime.runtimeId)
        {
            inv.ReturnItemToInventory(current);
        }

        inv.AssignWeaponToSlot(runtime, arrayIndex);
        UpdateSlot();

        base.OnDrop(eventData);
    }

    public override InventoryItemRuntime GetItem()
    {
        int index = GetWeaponArrayIndex();
        return PlayerInventory.Instance.weaponSlots[index];
    }

    public void UpdateSlot()
    {
        int index = GetWeaponArrayIndex();
        var weapon = PlayerInventory.Instance.weaponSlots[index];
        if (weapon != null)
        {
            SetItem(weapon);
        }
        else
        {
            Clear();
        }
    }
}