using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Sound sub keys (category is fixed = 'UI')")]
    public string hoverKey = "ButtonRollover";
    public string clickKey = "ButtonClick";
    public string releaseKey = "ButtonClickRelease";

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(hoverKey))
            AudioManager.Instance.Play("UI", hoverKey);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(clickKey))
            AudioManager.Instance.Play("UI", clickKey);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(releaseKey))
            AudioManager.Instance.Play("UI", releaseKey);
    }
}
