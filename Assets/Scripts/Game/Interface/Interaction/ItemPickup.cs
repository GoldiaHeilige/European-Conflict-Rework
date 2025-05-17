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

        if (itemData is WeaponData weaponData)
        {
            var ammo = PlayerInventory.Instance.GetDefaultAmmoFor(weaponData.weaponClass);
            var runtimeWeapon = new WeaponRuntimeItem(weaponData, ammo);
            success = PlayerInventory.Instance.AddUniqueItem(runtimeWeapon); // ✅ FIX
        }
        else if (itemData.stackable)
        {
            success = PlayerInventory.Instance.AddStackableItem(itemData, amount);
        }
        else
        {
            var item = InventoryItemFactory.Create(itemData, 1);
            success = PlayerInventory.Instance.AddUniqueItem(item);
        }

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
