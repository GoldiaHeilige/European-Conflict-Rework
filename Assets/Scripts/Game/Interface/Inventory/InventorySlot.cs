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

        var sourceItem = sourceSlot.GetItem();
        var targetItem = this.GetItem();
        var inventory = PlayerInventory.Instance.items;

        // Chỉ xử lý nếu slot này là slot inventory (không phải ArmorSlotUI)
        if (this is ArmorSlotUI) return;

        int targetIndex = this.slotIndex;

        if (targetItem == null)
        {
            inventory[targetIndex] = sourceItem;

            // Nếu source là slot trong kho thì clear nó
            if (!(sourceSlot is ArmorSlotUI))
            {
                inventory[sourceSlot.slotIndex] = null;
            }
            else
            {
                // Nếu kéo từ giáp về kho, gỡ khỏi giáp
                var player = GameObject.FindWithTag("Player");
                var armorManager = player?.GetComponent<EquippedArmorManager>();
                if (armorManager != null && sourceSlot is ArmorSlotUI armorSlot)
                {
                    armorManager.RemoveArmor(armorSlot.armorSlotType);
                    armorSlot.UpdateSlot(); // cập nhật lại slot giáp
                }
            }
        }
        else
        {
            if (sourceSlot is ArmorSlotUI armorSlot)
            {
                // 🧠 Gỡ A khỏi slot giáp
                var player = GameObject.FindWithTag("Player");
                var armorManager = player?.GetComponent<EquippedArmorManager>();
                if (armorManager != null)
                {
                    // Lưu lại B
                    var armorA = armorManager.GetArmor(armorSlot.armorSlotType);
                    var itemA = armorA?.sourceItem;
                    var itemB = targetItem;

                    // 1. Gán B (trong kho) lên slot giáp
                    if (itemB is InventoryItemRuntime bItem)
                    {
                        var armorData = bItem.itemData as ArmorData;
                        var newArmorRuntime = new ArmorRuntime(armorData, armorManager, bItem);
                        armorManager.EquipArmor(newArmorRuntime, armorSlot);
                        armorSlot.UpdateSlot();
                    }

                    // 2. Gán A (giáp đang mặc) vào inventory
                    inventory[targetIndex] = itemA;
                }
            }
            else
            {
                // 💡 swap bình thường giữa inventory ↔ inventory
                inventory[sourceSlot.slotIndex] = targetItem;
                inventory[targetIndex] = sourceItem;
            }
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
}