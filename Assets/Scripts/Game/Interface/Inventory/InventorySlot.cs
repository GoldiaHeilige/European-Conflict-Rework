using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameObject iconHolderPrefab;
    protected GameObject currentIconObj;

    protected InventoryItemRuntime currentItem;
    public int slotIndex;

    public virtual void SetItem(InventoryItemRuntime item)
    {
        Clear(); // remove old UI

        currentItem = item; // chỉ gán reference

        if (item == null || item.itemData == null)
        {
            Clear();
            return;
        }

        if (iconHolderPrefab == null) return;

        currentIconObj = Instantiate(iconHolderPrefab, transform);

        RectTransform rt = currentIconObj.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.SetParent(transform, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localScale = Vector3.one;

            if (this is ArmorSlotUI armorSlot)
                rt.localScale = armorSlot.armorSlotType == ArmorSlot.Head
                    ? new Vector3(1.8f, 1.8f, 1)
                    : new Vector3(1.5f, 1.5f, 1);
        }

        Image img = currentIconObj.GetComponentInChildren<Image>();
        if (img != null)
            img.sprite = item.itemData.icon;

        var text = currentIconObj.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = item.itemData.stackable && item.quantity > 1 ? item.quantity.ToString() : "";
    }

    public virtual void Clear()
    {
/*        Debug.Log($"[UI] Clear Slot {slotIndex} vì item null");*/

        currentItem = null;
        if (currentIconObj != null)
        {
            Destroy(currentIconObj);
            currentIconObj = null;
        }
    }

    public virtual InventoryItemRuntime GetItem() => currentItem;
    public bool HasItem() => currentItem != null;

    public virtual void OnDrop(PointerEventData eventData)
    {
        var sourceSlot = InventorySlotDragHandler.currentDraggingSlot;
        if (sourceSlot == null || sourceSlot == this) return;

        // ❌ Không xử lý nếu đang thả vào ArmorSlot hoặc WeaponSlot
        if (this is ArmorSlotUI || this is WeaponSlotUI) return;

        var targetIndex = this.slotIndex;
        var inventory = PlayerInventory.Instance.items;

        // ✅ Nếu kéo từ WeaponSlotUI về kho
        if (sourceSlot is WeaponSlotUI weaponSlot)
        {
            int sourceWeaponIndex = weaponSlot.slotType == WeaponSlotType.Primary ? 0 : 1;
            var weapon = PlayerInventory.Instance.weaponSlots[sourceWeaponIndex];

            if (weapon != null)
            {
                // Xoá khỏi weaponSlots
                PlayerInventory.Instance.weaponSlots[sourceWeaponIndex] = null;

                // Gán bản gốc vào inventory
                inventory[targetIndex] = weapon;
                Debug.Log($"[OnDrop] Đưa vũ khí từ weapon slot {sourceWeaponIndex} về inventory slot {targetIndex}");

                PlayerInventory.Instance.RaiseInventoryChanged("Kéo vũ khí từ weapon slot về kho");
            }

            return; // ❗ Ngăn xử lý tiếp
        }

        // ✳️ Nếu không phải từ WeaponSlot → xử lý mặc định
        var sourceItem = sourceSlot.GetItem();
        var targetItem = this.GetItem();

        if (targetItem == null)
        {
            inventory[targetIndex] = sourceItem;
            inventory[sourceSlot.slotIndex] = null;
        }
        else
        {
            inventory[sourceSlot.slotIndex] = targetItem;
            inventory[targetIndex] = sourceItem;
        }

        PlayerInventory.InventoryChanged?.Invoke();
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

    public InventoryItemRuntime GetItemFromInventorySlot()
    {
        return PlayerInventory.Instance.items[slotIndex];
    }

}