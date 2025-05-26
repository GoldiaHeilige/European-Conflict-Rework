using UnityEngine;
using UnityEngine.UI;

public class DragPreviewSystem : MonoBehaviour
{
    public static DragPreviewSystem Instance { get; private set; }

    [SerializeField] private GameObject previewObject;
    [SerializeField] private Image previewImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Hide();
    }

    public void Show(Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogWarning("DragPreview sprite null!");
            return;
        }

        previewObject.SetActive(true);
        previewImage.sprite = sprite;

        Color c = previewImage.color;
        c.a = 1f;
        previewImage.color = c;
    }


    public void MoveTo(Vector2 position)
    {
        int offsetY = 0;
        RectTransform canvasRect = previewObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            position,
            null, // null nếu Canvas là Screen Space Overlay
            out var localPoint
        );
        previewObject.GetComponent<RectTransform>().anchoredPosition = localPoint + new Vector2(0, offsetY);
    }

    public void Hide()
    {
        previewObject.SetActive(false);
    }
}
