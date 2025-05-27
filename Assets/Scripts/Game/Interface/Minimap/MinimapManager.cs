using UnityEngine;
using System.Collections.Generic;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;

    [Header("References")]
    public RectTransform minimapRect;
    public Transform worldOrigin;
    public Vector2 worldSize;
    public Transform playerTransform;

    [Header("Icon Prefabs")]
    public List<MinimapIconData> iconPrefabs;

    private Dictionary<MinimapTarget, RectTransform> iconDict = new();

    void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        foreach (var kvp in iconDict)
        {
            UpdateIconPosition(kvp.Key);
        }
    }

    public void RegisterTarget(MinimapTarget target)
    {
        var prefab = GetIconPrefab(target.iconInfo);
        if (prefab == null) return;

        var icon = MinimapIconPool.Instance.GetIcon(target.iconInfo);
        if (icon == null) return;

        icon.SetParent(minimapRect, false); // Gắn vào nơi hiển thị
        target.minimapIcon = icon;
        iconDict[target] = icon;
    }

    public void UnregisterTarget(MinimapTarget target)
    {
        if (iconDict.TryGetValue(target, out var icon))
        {
            // CHẶN truy cập nếu icon đã bị hủy
            if (icon == null || icon.Equals(null)) return;

            MinimapIconPool.Instance.ReturnIcon(target.iconInfo, icon.gameObject);
            iconDict.Remove(target);
        }
    }


    private GameObject GetIconPrefab(MinimapIconInfo info)
    {
        foreach (var data in iconPrefabs)
        {
            if (data.team == info.team && data.type == info.type)
                return data.prefab;
        }
        return null;
    }

    private void UpdateIconPosition(MinimapTarget target)
    {
        Vector3 delta = target.transform.position - playerTransform.position;

        float xRatio = -delta.x / worldSize.x;
        float yRatio = -delta.y / worldSize.y;

        Vector2 offset = new Vector2(
            xRatio * minimapRect.sizeDelta.x,
            yRatio * minimapRect.sizeDelta.y
        );

        target.minimapIcon.anchoredPosition = offset;
    }
}
