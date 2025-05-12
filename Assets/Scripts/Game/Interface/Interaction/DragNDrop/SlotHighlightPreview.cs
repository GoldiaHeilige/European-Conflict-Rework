using UnityEngine;
using UnityEngine.UI;

public class SlotHighlightPreview : MonoBehaviour
{
    public static SlotHighlightPreview Instance;

    [SerializeField] private GameObject previewObject;
    [SerializeField] private Image previewIcon;
    [SerializeField] private Image swapOverlay;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Hide();
    }

    public void Show(Sprite icon, bool isSwap)
    {
        previewObject.SetActive(true);
        previewIcon.sprite = icon;
        previewIcon.color = new Color(1, 1, 1, 0.5f); 

        swapOverlay.gameObject.SetActive(isSwap);
    }

    public void MoveTo(Vector3 position)
    {
        previewObject.transform.position = position;
    }

    public void Hide()
    {
        previewObject.SetActive(false);
    }
}
