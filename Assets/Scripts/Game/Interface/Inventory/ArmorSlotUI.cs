using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class ArmorSlotUI : InventorySlot
{
    public ArmorSlot armorSlotType;
    [SerializeField] private TextMeshProUGUI durabilityText;

    public override void OnDrop(PointerEventData eventData)
    {
        var dragSource = InventorySlotDragHandler.currentDraggingSlot;
        if (dragSource == null || dragSource == this)
            return;

        var draggedItem = dragSource.GetItem();
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

        // 🔁 Nếu chưa phải ArmorRuntimeItem → tạo mới bản runtime
        ArmorRuntimeItem armorItem = draggedItem as ArmorRuntimeItem;
        if (armorItem == null)
        {
            armorItem = new ArmorRuntimeItem(
                armorData,
                durability: draggedItem.durability,
                forcedId: draggedItem.runtimeId
            );
        }

        inv.RemoveExactItem(draggedItem); // xóa bản cũ

        // 🧼 Nếu đang mặc giáp khác → trả về kho
        var currentEquipped = armorManager.GetArmor(armorSlotType);
        if (currentEquipped != null && currentEquipped.sourceItem.runtimeId != armorItem.runtimeId)
        {
            inv.ReturnItemToInventory(currentEquipped.sourceItem);
        }

        // ✅ Gán bản runtime cho hệ thống mặc
        var armorRuntime = new ArmorRuntime(armorData, armorManager, armorItem);
        armorManager.EquipArmor(armorRuntime, this);
        FindObjectOfType<WeightDisplayHUD>(true)?.ForceUpdate();

        UpdateSlot();
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

        if (armor != null)
        {
            SetItem(armor.sourceItem);

            var armorData = armor.armorData;
            int current = armor.currentDurability;
            int max = armorData.maxDurability;

            durabilityText.text = $"{current} / {max}";

            float percent = max > 0 ? current / (float)max : 0f;

            if (percent == 0f)
                durabilityText.color = Color.red;
            else if (percent <= 0.25f)
                durabilityText.color = new Color(1f, 0.5f, 0f); // cam
            else if (percent <= 0.5f)
                durabilityText.color = Color.yellow;
            else
                durabilityText.color = Color.white;
        }
        else
        {
            durabilityText.text = "--- / ---";
            Clear();
        }
    }


    private void OnEnable()
    {
        PlayerInventory.InventoryChanged -= UpdateSlot;
        PlayerInventory.InventoryChanged += UpdateSlot;

        ArmorRuntime.OnAnyDurabilityChanged -= UpdateSlot;
        ArmorRuntime.OnAnyDurabilityChanged += UpdateSlot;

    }

    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= UpdateSlot;
        ArmorRuntime.OnAnyDurabilityChanged -= UpdateSlot;

    }
}
