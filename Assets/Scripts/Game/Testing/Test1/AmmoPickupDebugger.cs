using UnityEngine;

public class AmmoPickupDebugger : MonoBehaviour
{
    void Start()
    {
        var pickups = FindObjectsOfType<AmmoPickup>();
        foreach (var pickup in pickups)
        {
            var itemPickup = pickup.GetComponent<PickupItem>();
            if (itemPickup == null)
            {
                Debug.LogError($"[DEBUG] {pickup.name} thiếu PickupItem");
                continue;
            }

            if (itemPickup.itemData == null)
            {
                Debug.LogError($"[DEBUG] {pickup.name} thiếu itemData");
                continue;
            }

            var ammoItem = itemPickup.itemData as AmmoItemData;
            if (ammoItem == null)
            {
                Debug.LogError($"[DEBUG] {pickup.name} itemData KHÔNG PHẢI AmmoItemData ({itemPickup.itemData.GetType()})");
                continue;
            }

            if (ammoItem.linkedAmmoData == null)
            {
                Debug.LogError($"[DEBUG] {pickup.name} thiếu linkedAmmoData trong {ammoItem.name}");
            }
            else
            {
                Debug.Log($"[DEBUG] OK: {pickup.name} → {ammoItem.linkedAmmoData.ammoName}");
            }
        }
    }
}
