using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;
    [HideInInspector] public bool isHovered = false;

    private SpriteRenderer sr;
    private Color normalColor;
    public Color glowColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
    }

    private void Update()
    {
        if (isHovered) sr.color = glowColor;
        else sr.color = normalColor;

        isHovered = false;
    }

    public void Pickup()
    {
        bool success = false;

        if (itemData.stackable)
        {
            success = PlayerInventory.Instance.AddStackableItem(itemData, amount);
        }
        else
        {
            var item = InventoryItemFactory.Create(itemData, 1); // ✅ dùng Factory để đảm bảo runtimeId duy nhất
            success = PlayerInventory.Instance.AddUniqueItem(item);
        }

        GetComponent<AmmoPickup>()?.OnPickedUp();

        if (success)
        {
            PlayerInventory.InventoryChanged?.Invoke(); // đảm bảo luôn update
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Kho đầy, không thể nhặt item: " + itemData.itemID);
        }

        Debug.Log("[PICKUP] gọi xong AddUniqueItem, trước khi Destroy");
    }
}
