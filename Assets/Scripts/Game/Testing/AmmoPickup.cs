using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private PickupItem itemPickup;

    private void Awake()
    {
        itemPickup = GetComponent<PickupItem>();
    }

    public void OnPickedUp()
    {
        var itemData = itemPickup?.itemData as AmmoItemData;
        if (itemData == null || itemData.linkedAmmoData == null)
        {
            Debug.LogWarning($"[AmmoPickup] Không thể nhặt vì thiếu dữ liệu");
            return;
        }

        int amount = itemPickup?.amount ?? 1;
        PlayerInventory.Instance.AddAmmo(itemData.linkedAmmoData, amount);
        Debug.Log($"[AmmoPickup] Nhặt {amount} viên {itemData.linkedAmmoData.ammoName}");
    }
}
