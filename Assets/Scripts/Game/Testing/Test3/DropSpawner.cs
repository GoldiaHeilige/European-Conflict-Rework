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

        Vector3 position = dropOrigin.position;
        Spawn(itemRuntime, position, isRuntimeSource);

        if (itemRuntime?.itemData != null)
        {
            AudioManager.Instance?.Play(
                itemRuntime.itemData.dropCategory,
                itemRuntime.itemData.dropSubKey
            );
        }

    }

    public void Spawn(InventoryItemRuntime itemRuntime, Vector3 position, bool isRuntimeSource = false)
    {
        GameObject prefab = GetPrefabForItem(itemRuntime.itemData);
        if (prefab == null)
        {
            Debug.LogWarning("[DropSpawner] Không tìm thấy prefab phù hợp");
            return;
        }

        // Xác định vị trí và xoay ngẫu nhiên 1 chút
        Vector3 spawnPos = position + Vector3.right * Random.Range(-0.3f, 0.3f) + Vector3.up * 0.1f;
        float randomRotationZ = Random.Range(-25f, 25f);
        GameObject go = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 0f, randomRotationZ));

        // Gán GUID nếu là item runtime từ inventory
        if (isRuntimeSource)
        {
            var initializer = go.GetComponent<UniqueItemInitializer>();
            if (initializer != null)
            {
                initializer.itemGUID = itemRuntime.runtimeId;
            }
        }

        // Gán thông tin item vào PickupItem
        var pickup = go.GetComponent<PickupItem>();
        if (pickup != null)
        {
            pickup.runtimeItem = itemRuntime;
            pickup.itemData = itemRuntime.itemData;
            pickup.amount = itemRuntime.quantity;

            Debug.Log($"[DropSpawner] GÁN runtimeItem: {itemRuntime.runtimeId}, type = {itemRuntime.GetType().Name}, data = {itemRuntime.itemData?.itemID}");
        }

        // Gán sprite nếu có
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null && itemRuntime.itemData != null)
        {
            sr.sprite = itemRuntime.itemData.worldSprite;
        }

        // Hiệu ứng đạn súng
        if (itemRuntime is WeaponRuntimeItem weapon)
        {
            Debug.Log($"[DROP] Weapon drop → Clip: {weapon.ammoInClip}, Ammo: {weapon.currentAmmoType?.ammoName ?? "null"}");
        }

        // AddForce nhẹ văng ra ngoài
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized * Random.Range(1.5f, 3.5f);
            rb.AddForce(randomDir, ForceMode2D.Impulse);
        }

        // Tự hủy sau X giây nếu không nhặt
         float lifetime = 360f; // có thể tùy chỉnh theo config sau này
        AutoDestroy auto = go.GetComponent<AutoDestroy>();
        if (auto == null)
        {
            auto = go.AddComponent<AutoDestroy>();
            auto.lifetime = lifetime;
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
