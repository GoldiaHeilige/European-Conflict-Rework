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
            Debug.Log($"[InitGUID] Tạo GUID mới: {itemGUID}");
        }
        else
        {
            Debug.Log($"[InitGUID] Sử dụng GUID đã có: {itemGUID}");
        }
    }


    public string GetGUID()
    {
        return itemGUID;
    }
}
