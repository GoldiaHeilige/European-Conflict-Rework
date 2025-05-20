using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        amount = Mathf.Clamp(amount, 1, sourceItem.quantity);

        // Clone item mới
        InventoryItemRuntime dropped = new InventoryItemRuntime(
            sourceItem.itemData,
            amount,
            null,
            System.Guid.NewGuid().ToString()
        );

        // Trừ số lượng gốc
        sourceItem.quantity -= amount;

        if (sourceItem.quantity <= 0)
            PlayerInventory.Instance.RemoveExactItem(sourceItem);

        // Gọi DropSpawner
        DropSpawner.Instance.Spawn(dropped);

        Hide();
    }
}
