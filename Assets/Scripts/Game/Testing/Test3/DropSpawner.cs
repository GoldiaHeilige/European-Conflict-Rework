using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    public static DropSpawner Instance;

    [Header("Vị trí spawn")]
    public Transform dropOrigin;

    [Header("Prefabs theo loại item")]
    public GameObject ammoPickupPrefab;
    public GameObject weaponPickupPrefab;
    public GameObject armorPickupPrefab;
    public GameObject defaultPickupPrefab;

    private void Awake()
    {
        Instance = this;

        // Nếu quên gán → tự tìm Player làm origin
        if (dropOrigin == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                dropOrigin = player.transform;
        }
    }

    public void Spawn(InventoryItemRuntime itemRuntime, bool isRuntimeSource = false)
    {
        if (dropOrigin == null)
        {
            Debug.LogWarning("[DropSpawner] dropOrigin chưa gán!");
            return;
        }

        GameObject prefab = GetPrefabForItem(itemRuntime.itemData);
        if (prefab == null)
        {
            Debug.LogWarning("[DropSpawner] Không tìm thấy prefab phù hợp");
            return;
        }

        Vector3 spawnPos = dropOrigin.position + Vector3.right * Random.Range(-0.3f, 0.3f) + Vector3.up * 0.1f;

        GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (isRuntimeSource)
        {
            var initializer = go.GetComponent<UniqueItemInitializer>();
            if (initializer != null)
            {
                initializer.itemGUID = itemRuntime.runtimeId;

                // 💥 Force gọi lại Awake logic nếu cần (hoặc đảm bảo itemGUID không bị override)
            }
        }

        // Gán dữ liệu vào prefab mới
        var pickup = go.GetComponent<PickupItem>();
        if (pickup != null)
        {
            pickup.runtimeItem = itemRuntime; // ✅ PHẢI có dòng này
            pickup.itemData = itemRuntime.itemData;
            pickup.amount = itemRuntime.quantity;

            Debug.Log($"[DropSpawner] GÁN runtimeItem: {itemRuntime.runtimeId}, type = {itemRuntime.GetType().Name}, data = {itemRuntime.itemData?.itemID}");
        }


        // 🔥 Gán đúng sprite cho SpriteRenderer từ data
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null && itemRuntime.itemData != null)
        {
            sr.sprite = itemRuntime.itemData.worldSprite;
        }

        if (itemRuntime is WeaponRuntimeItem weapon)
        {
            Debug.Log($"[DROP] Weapon drop → Clip: {weapon.ammoInClip}, Ammo: {weapon.currentAmmoType?.ammoName ?? "null"}");
        }

    }


    private GameObject GetPrefabForItem(InventoryItemData itemData)
    {
        if (itemData is AmmoItemData) return ammoPickupPrefab;
        if (itemData is WeaponData) return weaponPickupPrefab;
        if (itemData is ArmorData) return armorPickupPrefab;

        return defaultPickupPrefab;
    }
}
