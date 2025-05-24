using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;
    [HideInInspector] public bool isHovered = false;

    [System.NonSerialized] public InventoryItemRuntime runtimeItem;

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
        Debug.Log($"[DEBUG] Pickup() bắt đầu — runtimeItem: {(runtimeItem != null ? "OK" : "NULL")} | itemData: {itemData?.itemID ?? "null"}");

        bool success = false;

        var initializer = GetComponent<UniqueItemInitializer>();
        string guid = initializer != null ? initializer.GetGUID() : null;

        InventoryItemRuntime item;

        if (runtimeItem != null && runtimeItem.itemData != null)
        {
            item = runtimeItem;
            Debug.Log($"[PICKUP] Nhặt lại item runtime: {item.runtimeId}");
            Debug.Log($"[DEBUG] Nhặt đúng runtimeItem: {runtimeItem.runtimeId} | Clip: {(runtimeItem is WeaponRuntimeItem w ? w.ammoInClip.ToString() : "NA")}");

        }
        else
        {
            item = InventoryItemFactory.Create(itemData, amount, log: false, forcedId: guid);
            Debug.LogWarning("[PICKUP] runtimeItem null hoặc thiếu itemData → tạo lại từ itemData");
        }

        if (item == null || item.itemData == null)
        {
            Debug.LogError("[PICKUP] Item hoặc itemData null → không thể nhặt");
            return;
        }

        if (item.itemData.stackable)
            success = PlayerInventory.Instance.AddStackableItem(item);
        else
            success = PlayerInventory.Instance.AddUniqueItem(item);

        if (item is WeaponRuntimeItem weapon)
        {
            Debug.Log($"[PICKUP] Weapon pickup → Clip: {weapon.ammoInClip}, Ammo: {weapon.currentAmmoType?.ammoName ?? "null"}");
        }

        GetComponent<AmmoPickup>()?.OnPickedUp();

        if (success)
        {
            PlayerInventory.InventoryChanged?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[PICKUP] Kho đầy hoặc không thể nhặt item: {item.itemData.itemID}");
        }

/*        Debug.Log("[PICKUP] Đã xử lý Pickup()");*/
    }
}