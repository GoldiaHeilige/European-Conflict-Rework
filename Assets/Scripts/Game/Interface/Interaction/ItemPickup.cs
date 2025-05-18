using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;
    [HideInInspector] public bool isHovered = false;

    private SpriteRenderer sr;
    private Color normalColor;
    public Color glowColor = Color.yellow;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
    }

    private void Update()
    {
        sr.color = isHovered ? glowColor : normalColor;
        isHovered = false;
    }

    public void Pickup()
    {
        bool success = false;

        // preview tạo sẵn item (không cần dùng, chỉ để gọi stack check)
        var item = InventoryItemFactory.Create(itemData, amount, log: false);

        if (itemData.stackable)
            success = PlayerInventory.Instance.AddStackableItem(itemData, amount);
        else
            success = PlayerInventory.Instance.AddUniqueItem(item);

        GetComponent<AmmoPickup>()?.OnPickedUp();

        if (success)
        {
            PlayerInventory.InventoryChanged?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[PICKUP] Kho đầy hoặc không thể nhặt item: {itemData.itemID}");
        }

        Debug.Log("[PICKUP] Đã xử lý Pickup()");
    }

}