using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected GameObject iconHolderPrefab;
    protected GameObject currentIconObj;
    public GameObject popupPrefab;
    public Transform canvasTransform;

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

        // Không xử lý nếu đang thả vào ArmorSlot hoặc WeaponSlot
        if (this is ArmorSlotUI || this is WeaponSlotUI) return;

        var targetIndex = this.slotIndex;
        var inventory = PlayerInventory.Instance.items;

        // Kéo từ WeaponSlot về kho
        if (sourceSlot is WeaponSlotUI weaponSlot)
        {
            int sourceWeaponIndex = weaponSlot.slotType == WeaponSlotType.Primary ? 0 : 1;
            var weapon = PlayerInventory.Instance.weaponSlots[sourceWeaponIndex];

            if (weapon != null)
            {
                if (PlayerInventory.Instance.equippedWeapon != null &&
                    PlayerInventory.Instance.equippedWeapon.runtimeId == weapon.runtimeId)
                {
/*                    Debug.Log("[InventorySlot] Vũ khí đang được trang bị → huỷ trang bị");*/
                    PlayerInventory.Instance.equippedWeapon = null;
                    PlayerInventory.Instance.currentWeaponIndex = -1;

                    PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
                    PlayerInventory.Instance.modelViewer?.UpdateSprite(null);

                    if (PlayerWeaponCtrl.Instance != null)
                        PlayerWeaponCtrl.Instance.ClearWeapon();
                }

                PlayerInventory.Instance.weaponSlots[sourceWeaponIndex] = null;
                inventory[targetIndex] = weapon;

/*                Debug.Log($"[OnDrop] Đưa vũ khí từ weapon slot {sourceWeaponIndex} về inventory slot {targetIndex}");*/
                PlayerInventory.Instance.RaiseInventoryChanged("Kéo vũ khí từ weapon slot về kho");
            }

            return;
        }

        // Kéo từ ArmorSlot về kho
        if (sourceSlot is ArmorSlotUI armorSlot)
        {
            var player = GameObject.FindWithTag("Player");
            var armorManager = player?.GetComponent<EquippedArmorManager>();

            // Lấy item trước khi gỡ
            var armorItem = sourceSlot.GetItem();

            if (armorManager != null)
            {
                Debug.Log($"[InventorySlot] Gỡ giáp khỏi slot {armorSlot.armorSlotType}");
                armorManager.RemoveArmor(armorSlot.armorSlotType);
            }

            if (armorItem != null)
            {
                inventory[targetIndex] = armorItem; // Đưa vào kho
                Debug.Log($"[OnDrop] Đưa giáp về inventory slot {targetIndex}");

                PlayerInventory.Instance.RaiseInventoryChanged("Kéo giáp từ ArmorSlot về kho");
            }

            return;
        }

        // ✳️ Nếu không phải từ slot đặc biệt → xử lý mặc định
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (currentItem == null)
            return; // Không có item thì bỏ qua

        if (popupPrefab == null || canvasTransform == null)
        {
            Debug.LogError($"[InventorySlot] popupPrefab hoặc canvasTransform chưa được gán tại slot {slotIndex}");
            return;
        }

        GameObject popupGO = Instantiate(popupPrefab, canvasTransform);
        var popup = popupGO.GetComponent<ItemContextMenu>();
        if (popup == null)
        {
            Debug.LogError("[InventorySlot] Không tìm thấy ItemContextMenu trên popupPrefab");
            return;
        }

        RectTransform slotRect = GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        slotRect.GetWorldCorners(corners);
        Vector3 worldPos = corners[2];
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);

        popup.Setup(currentItem, screenPos, canvasTransform.GetComponent<Canvas>());
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