using UnityEngine;
using System;

public class UniqueItemInitializer : MonoBehaviour
{
    public string itemGUID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(itemGUID))
        {
            itemGUID = Guid.NewGuid().ToString();
            Debug.Log($"[InitGUID] Spawned item có GUID: {itemGUID}");
        }
    }

    public string GetGUID()
    {
        return itemGUID;
    }
}
