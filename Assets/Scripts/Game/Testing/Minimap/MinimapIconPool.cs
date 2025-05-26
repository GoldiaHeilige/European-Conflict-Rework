using UnityEngine;
using System.Collections.Generic;

public class MinimapIconPool : MonoBehaviour
{
    [System.Serializable]
    public class IconPoolEntry
    {
        public MinimapIconInfo info;
        public GameObject prefab;
        public Transform poolRoot;

        [HideInInspector] public Queue<GameObject> pool = new();
    }

    public static MinimapIconPool Instance;

    public List<IconPoolEntry> iconPools;

    void Awake()
    {
        Instance = this;

        foreach (var pool in iconPools)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject icon = Instantiate(pool.prefab, pool.poolRoot);
                icon.SetActive(false);
                pool.pool.Enqueue(icon);
            }
        }

    }

    public RectTransform GetIcon(MinimapIconInfo info)
    {
        var pool = iconPools.Find(p => p.info.team == info.team && p.info.type == info.type);
        if (pool == null) return null;

        GameObject go;
        if (pool.pool.Count > 0)
        {
            go = pool.pool.Dequeue();
        }
        else
        {
            go = Instantiate(pool.prefab);
        }

        go.SetActive(true);
        return go.GetComponent<RectTransform>();
    }


    public void ReturnIcon(MinimapIconInfo info, GameObject icon)
    {
        var pool = iconPools.Find(p => p.info.team == info.team && p.info.type == info.type);
        if (pool == null)
        {
            Destroy(icon); return;
        }

        icon.SetActive(false);
        icon.transform.SetParent(pool.poolRoot, false);
        pool.pool.Enqueue(icon);
    }
}
