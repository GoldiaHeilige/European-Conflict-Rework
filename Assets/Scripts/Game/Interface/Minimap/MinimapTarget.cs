using UnityEngine;
using System.Collections;

public class MinimapTarget : MonoBehaviour
{
    public MinimapIconInfo iconInfo;
    [HideInInspector] public RectTransform minimapIcon;
    private bool isRegistered = false;

    void OnEnable()
    {
        StartCoroutine(RegisterNextFrame());
    }

    IEnumerator RegisterNextFrame()
    {
        yield return null; // Đảm bảo MinimapManager đã Awake
        if (!isRegistered && MinimapManager.Instance != null)
        {
            MinimapManager.Instance.RegisterTarget(this);
            isRegistered = true;
        }
    }

    void OnDisable()
    {
        if (isRegistered && MinimapManager.Instance != null && MinimapIconPool.Instance != null)
        {
            MinimapManager.Instance.UnregisterTarget(this);
            isRegistered = false;
        }
    }

    void OnDestroy()
    {
        if (isRegistered && MinimapManager.Instance != null && MinimapIconPool.Instance != null)
        {
            MinimapManager.Instance.UnregisterTarget(this);
        }
    }

}
