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

        InventoryItemRuntime draggedItem;

        // ✅ Lấy bản gốc từ inventory nếu kéo từ kho
        if (dragSource.slotIndex < PlayerInventory.Instance.maxSlotDisplay)
        {
            draggedItem = dragSource.GetItemFromInventorySlot();
        }
        else
        {
            draggedItem = dragSource.GetItem();
        }

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

        var inv = PlayerInventory.Instance;
        int arrayIndex = GetWeaponArrayIndex();

        // ❗ 1. Loại bỏ bản trùng ở items[] (kho)
        inv.RemoveExactItem(runtime);

        // ❗ 2. Loại bỏ bản trùng ở mọi weaponSlots
        for (int i = 0; i < inv.weaponSlots.Length; i++)
        {
            if (inv.weaponSlots[i] != null && inv.weaponSlots[i].runtimeId == runtime.runtimeId)
            {
                inv.weaponSlots[i] = null;
                Debug.Log($"[WeaponSlotUI] Xoá weaponSlots[{i}] vì trùng GUID");
            }
        }

        // ❗ 3. Nếu đang có vũ khí khác trong slot → trả lại về kho
        var current = inv.weaponSlots[arrayIndex];
/*        Debug.Log($"[DEBUG] So sánh: equipped {inv.equippedWeapon?.runtimeId} vs runtime {runtime.runtimeId}");*/

        if (inv.equippedWeapon != null && inv.equippedWeapon.runtimeId == runtime.runtimeId)
        {
            inv.ReturnItemToInventory(current);
        }

        if (inv.equippedWeapon != null && inv.equippedWeapon.guid == runtime.guid)
        {
            inv.equippedWeapon = null;
            inv.currentWeaponIndex = -1;
            inv.modelViewer?.UpdateSprite(null);

            if (PlayerWeaponCtrl.Instance != null)
            {
                PlayerWeaponCtrl.Instance.ClearWeapon();
            }
            else
            {
                Debug.LogError("[PlayerInventory] PlayerWeaponCtrl.Instance == null → KHÔNG GỌI ĐƯỢC");
            }
        }


        // ❗ 4. Gán vào slot đúng
        inv.AssignWeaponToSlot(runtime, arrayIndex);
        UpdateSlot();

        // ✅ Debug inventory sau cùng
        inv.PrintInventoryDebug("SAU DROP");

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

    private void OnEnable()
    {
        PlayerInventory.InventoryChanged += UpdateSlot;
    }

    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= UpdateSlot;
    }

}
