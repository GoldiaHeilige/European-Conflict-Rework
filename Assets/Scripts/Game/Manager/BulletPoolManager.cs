
using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        public GameObject bulletPrefab;
        public ObjectPool pool;
    }

    [SerializeField] private List<PoolEntry> bulletPools = new();

    public ObjectPool GetPoolForPrefab(GameObject bulletPrefab)
    {
        foreach (var entry in bulletPools)
        {
            if (entry.bulletPrefab == bulletPrefab)
                return entry.pool;
        }

        Debug.LogWarning($"Không tìm thấy Pool cho {bulletPrefab.name}");
        return null;
    }
}
