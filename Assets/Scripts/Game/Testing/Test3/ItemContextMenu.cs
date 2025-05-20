using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemContextMenu : MonoBehaviour
{
    public Button dropButton;
    public Button dropAmountButton;
    public Button destroyButton;

    private InventoryItemRuntime currentItem;
    private RectTransform rectTransform;

    public static ItemContextMenu currentOpenMenu;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(InventoryItemRuntime item, Vector2 screenPosition, Canvas canvas)
    {
        currentItem = item;

        // Clear menu trước đó
        if (currentOpenMenu != null && currentOpenMenu != this)
            Destroy(currentOpenMenu.gameObject);
        currentOpenMenu = this;

        // Bắt đầu ẩn toàn bộ button
        dropButton.interactable = false;
        dropAmountButton.interactable = false;
        destroyButton.interactable = false;

        // Gán visibility trước (để layout chạy đúng)
        bool isAmmo = item.itemData is AmmoItemData;
        dropButton.gameObject.SetActive(true);
        dropAmountButton.gameObject.SetActive(isAmmo);
        destroyButton.gameObject.SetActive(true);

        // Gán sự kiện
        dropButton.onClick.AddListener(() => {
            Debug.Log("[DROP] Drop toàn bộ");

            DropSpawner.Instance.Spawn(currentItem);
            PlayerInventory.Instance.RemoveByGUID(currentItem.runtimeId);
            Close();
        });



        dropAmountButton.onClick.AddListener(() => {
            DropAmountPopup found = FindObjectOfType<DropAmountPopup>(true); // true = tìm cả inactive
            if (found != null)
            {
                found.gameObject.SetActive(true);
                found.Show(currentItem);
            }
            else
            {
                Debug.LogError("[DropPopup] Không tìm thấy DropAmountPopup bằng FindObjectOfType");
            }


            Close(); // đóng popup context
        });


        destroyButton.onClick.RemoveAllListeners();
        destroyButton.onClick.AddListener(() => {
            Debug.Log("[DESTROY] Xoá item");
            PlayerInventory.Instance.DestroyByGUID(item.runtimeId);
            Close();
        });

        // Set vị trí popup
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );
            transform.localPosition = localPoint;
        }
        else
        {
            transform.position = screenPosition;
        }

        ClampToCanvas(canvas);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        // Force clear selection
        EventSystem.current.SetSelectedGameObject(null);

        // Delay bật tương tác sang frame sau
        StartCoroutine(EnableInteractionNextFrame());
    }



    private void ClampToCanvas(Canvas canvas)
    {
        RectTransform canvasRect = canvas.transform as RectTransform;
        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 popupSize = rectTransform.sizeDelta;
        Vector2 pos = rectTransform.anchoredPosition;

        float halfW = popupSize.x / 2;
        float halfH = popupSize.y / 2;

        float maxX = canvasSize.x / 2 - halfW;
        float minX = -canvasSize.x / 2 + halfW;
        float maxY = canvasSize.y / 2 - halfH;
        float minY = -canvasSize.y / 2 + halfH;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rectTransform.anchoredPosition = pos;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check nếu chuột nằm ngoài toàn bộ popup
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
            {
                // Đồng thời không hover vào bất kỳ Button nào
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                bool clickedInsideButton = false;
                foreach (var r in results)
                {
                    if (r.gameObject.transform.IsChildOf(transform)) // nếu thuộc popup
                    {
                        clickedInsideButton = true;
                        break;
                    }
                }

                if (!clickedInsideButton)
                {
                    Close();
                }
            }
        }
    }


    public void Close()
    {
        if (currentOpenMenu == this)
            currentOpenMenu = null;

        Destroy(gameObject);
    }

    private System.Collections.IEnumerator ForceRefreshInput()
    {
        // Đợi 1 frame để UI kịp instantiate xong
        yield return null;

        // Fake 1 sự kiện pointer để hệ thống UI reset
        PointerEventData pointer = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        // Nếu cần: log xem có button nào dưới chuột
        foreach (var r in results)
        {
            Debug.Log($"[ForceRefreshInput] UI hit: {r.gameObject.name}");
        }
    }

    private System.Collections.IEnumerator EnableInteractionNextFrame()
    {
        yield return null; // đợi 1 frame

        dropButton.interactable = true;
        dropAmountButton.interactable = true;
        destroyButton.interactable = true;

        Debug.Log("[Popup] Buttons đã được bật tương tác.");
    }


}
