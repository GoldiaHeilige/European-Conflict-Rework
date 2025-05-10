using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    private InventoryItemRuntime currentItem;

    public void SetItem(InventoryItemRuntime item)
    {
        currentItem = item;
        iconImage.sprite = item.itemData.icon;
        iconImage.enabled = true;

        quantityText.text = (item.itemData.stackable && item.quantity > 1)
            ? item.quantity.ToString()
            : "";
    }

    public void Clear()
    {
        currentItem = null;
        iconImage.enabled = false;
        quantityText.text = "";
    }
}
