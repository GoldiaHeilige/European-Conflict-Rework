using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DropAmountPopup : MonoBehaviour
{
    public TMP_InputField quantityInput;
    public Button confirmButton;
    public Button cancelButton;
    public TextMeshProUGUI titleText;

    private InventoryItemRuntime sourceItem;

    public static DropAmountPopup Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(InventoryItemRuntime item)
    {
        sourceItem = item;
        titleText.text = $"Drop how many? (max {item.quantity})";
        quantityInput.text = "1";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(ConfirmDrop);
        cancelButton.onClick.AddListener(Hide);
    }

    private void ConfirmDrop()
    {
        if (sourceItem == null) return;

        int amount = int.Parse(quantityInput.text);
        if (amount <= 0)
        {
            Debug.LogWarning("[DropAmountPopup] Nhập số lượng <= 0 → Bỏ qua");
            Hide();
            return;
        }

        amount = Mathf.Clamp(amount, 1, sourceItem.quantity);

        bool droppedFull = amount == sourceItem.quantity;

        if (droppedFull)
        {
            InventoryItemRuntime toDrop = sourceItem;

            // Nếu là giáp nhưng chưa phải ArmorRuntimeItem → tạo đúng runtime
            if (sourceItem.itemData is ArmorData armorData && sourceItem is not ArmorRuntimeItem)
            {
                toDrop = new ArmorRuntimeItem(
                    armorData,
                    sourceItem.durability,
                    sourceItem.runtimeId
                );
            }

            DropSpawner.Instance.Spawn(toDrop, isRuntimeSource: true);
            PlayerInventory.Instance.RemoveExactItem(sourceItem);
        }
        else
        {
            // ✅ Tạo bản clone độc lập để drop
            InventoryItemRuntime droppedClone = new InventoryItemRuntime(
                sourceItem.itemData,
                amount,
                null,
                System.Guid.NewGuid().ToString()
            );
            DropSpawner.Instance.Spawn(droppedClone, isRuntimeSource: false);

            // ✅ Trừ trong kho đúng cách
            PlayerInventory.Instance.RemoveFromSpecificStack(sourceItem, amount);
        }

        // ✅ Cập nhật ammo UI nếu là đạn
        if (sourceItem.itemData is AmmoItemData ammoItem)
        {
            var matched = PlayerInventory.Instance.knownAmmoTypes
                .FirstOrDefault(a => a.ammoName == ammoItem.linkedAmmoData.ammoName);

            if (matched != null)
            {
                var equipped = PlayerWeaponCtrl.Instance?.runtimeItem;
                if (equipped != null && equipped.currentAmmoType == ammoItem.linkedAmmoData)
                {
                    equipped.CheckAmmoValid();

                    if (equipped.currentAmmoType.GetInstanceID() != matched.GetInstanceID())
                    {
                        Debug.LogWarning("[DropAmount] currentAmmoType đang giữ bản lệch → cập nhật lại");
                        equipped.currentAmmoType = matched;
                    }
                }

                PlayerWeaponCtrl.Instance?.ammoUI?.Refresh();
            }
        }

        var hud = FindObjectOfType<WeightDisplayHUD>(true);
        if (hud != null) hud.ForceUpdate();

        Hide();
    }

}
