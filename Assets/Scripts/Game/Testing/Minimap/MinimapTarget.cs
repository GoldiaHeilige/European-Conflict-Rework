using UnityEngine;

public class MinimapTarget : MonoBehaviour
{
    public MinimapIconInfo iconInfo;
    [HideInInspector] public RectTransform minimapIcon;

    void OnEnable()
    {
        StartCoroutine(RegisterNextFrame());
    }

    System.Collections.IEnumerator RegisterNextFrame()
    {
        yield return null; // Chờ 1 frame
        if (MinimapManager.Instance != null)
        {
            MinimapManager.Instance.RegisterTarget(this);
        }
    }

    void OnDestroy()
    {
        if (MinimapManager.Instance != null)
        {
            MinimapManager.Instance.UnregisterTarget(this);
        }
    }

}