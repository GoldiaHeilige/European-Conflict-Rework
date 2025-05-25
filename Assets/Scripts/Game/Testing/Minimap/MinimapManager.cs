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

        var icon = Instantiate(prefab, minimapRect);
        target.minimapIcon = icon.GetComponent<RectTransform>();
        iconDict[target] = target.minimapIcon;
    }

    public void UnregisterTarget(MinimapTarget target)
    {
        if (iconDict.TryGetValue(target, out var icon))
        {
            Destroy(icon.gameObject);
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
