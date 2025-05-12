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
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            previewObject.transform.parent as RectTransform,
            position,
            Camera.main,
            out localPoint
        );
        previewObject.GetComponent<RectTransform>().anchoredPosition = localPoint + new Vector2(0, offsetY);

    }

    public void Hide()
    {
        previewObject.SetActive(false);
    }
}
