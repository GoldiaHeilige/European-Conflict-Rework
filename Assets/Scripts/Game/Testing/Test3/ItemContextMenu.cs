
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ItemContextMenu : MonoBehaviour
{
    public Button dropButton;
    public Button dropAmountButton;
    public Button destroyButton;

    private InventoryItemRuntime currentItem;
    private RectTransform rectTransform;
    private int currentSlotIndex = -1;

    public static ItemContextMenu currentOpenMenu;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(InventoryItemRuntime item, int slotIndex, Vector2 screenPosition, Canvas canvas)
    {
        currentItem = item;
        currentSlotIndex = slotIndex;

        if (currentOpenMenu != null && currentOpenMenu != this)
            Destroy(currentOpenMenu.gameObject);
        currentOpenMenu = this;

        dropButton.interactable = false;
        dropAmountButton.interactable = false;
        destroyButton.interactable = false;

        bool isAmmo = item.itemData is AmmoItemData;
        dropButton.gameObject.SetActive(true);
        dropAmountButton.gameObject.SetActive(isAmmo);
        destroyButton.gameObject.SetActive(true);

        dropButton.onClick.AddListener(() => {
            Debug.Log("[DROP] Drop toàn bộ");

            // ✅ Kiểm tra nếu đang trang bị vũ khí hoặc giáp
            if (currentItem is WeaponRuntimeItem wpn)
            {
                // Nếu đang cầm (equippedWeapon) → gỡ vũ khí đang cầm
                if (PlayerInventory.Instance.equippedWeapon != null &&
                    PlayerInventory.Instance.equippedWeapon.runtimeId == currentItem.runtimeId)
                {
                    Debug.Log("[ItemContextMenu] Gỡ súng đang cầm");
                    PlayerInventory.Instance.UnequipWeapon();
                }

                // Nếu nằm trong weaponSlots[] → gỡ slot phù hợp
                for (int i = 0; i < PlayerInventory.Instance.weaponSlots.Length; i++)
                {
                    var slotWpn = PlayerInventory.Instance.weaponSlots[i];
                    if (slotWpn != null && slotWpn.runtimeId == currentItem.runtimeId)
                    {
                        Debug.Log($"[ItemContextMenu] Gỡ súng khỏi weaponSlots[{i}]");
                        PlayerInventory.Instance.UnequipWeaponSlot(i); // ← gọi đúng logic
                    }
                }
            }

            if (currentItem.itemData is ArmorData armorData)
            {
                var eq = FindObjectOfType<EquippedArmorManager>();
                var equipped = eq?.GetArmor(armorData.armorSlot);

                if (equipped != null && equipped.sourceItem.runtimeId == currentItem.runtimeId)
                {
                    Debug.Log("[ItemContextMenu] Drop giáp đang mặc → gỡ trang bị");
                    eq.RemoveArmor(armorData.armorSlot);
                }
            }

            DropSpawner.Instance.Spawn(currentItem, true);
            PlayerInventory.Instance.items[currentSlotIndex] = null;
            PlayerInventory.Instance.RaiseInventoryChanged("Drop item từ đúng slot");

            if (currentItem.itemData is AmmoItemData ammoItem)
            {
                var matched = PlayerInventory.Instance.knownAmmoTypes
                    .FirstOrDefault(a => a.ammoName == ammoItem.linkedAmmoData.ammoName);

                if (matched != null)
                {
                    var equipped = PlayerWeaponCtrl.Instance?.runtimeItem;
                    if (equipped != null && equipped.currentAmmoType == ammoItem.linkedAmmoData)
                    {
                        equipped.CheckAmmoValid();

                        if (equipped.currentAmmoType.GetInstanceID() != matched.GetInstanceID())
                        {
                            Debug.LogWarning("[Drop] currentAmmoType đang giữ bản lệch → cập nhật lại");
                            equipped.currentAmmoType = matched;
                        }
                    }
                }
            }

            Close();
        });

        dropAmountButton.onClick.AddListener(() => {
            DropAmountPopup found = FindObjectOfType<DropAmountPopup>(true);
            if (found != null)
            {
                found.gameObject.SetActive(true);
                found.Show(currentItem);
            }
            else
            {
                Debug.LogError("[DropPopup] Không tìm thấy DropAmountPopup bằng FindObjectOfType");
            }

            Close();
        });

        destroyButton.onClick.RemoveAllListeners();
        destroyButton.onClick.AddListener(() => {
            Debug.Log("[DESTROY] Xoá item");

            if (currentItem is WeaponRuntimeItem wpn &&
                PlayerInventory.Instance.equippedWeapon != null &&
                PlayerInventory.Instance.equippedWeapon.runtimeId == currentItem.runtimeId)
            {
                Debug.Log("[ItemContextMenu] Destroy vũ khí đang cầm → gỡ trang bị");
                PlayerInventory.Instance.UnequipWeapon();
                PlayerWeaponCtrl.Instance?.ClearWeapon();
            }

            if (currentItem.itemData is ArmorData armorData)
            {
                var eq = FindObjectOfType<EquippedArmorManager>();
                var equipped = eq?.GetArmor(armorData.armorSlot);

                if (equipped != null && equipped.sourceItem.runtimeId == currentItem.runtimeId)
                {
                    Debug.Log("[ItemContextMenu] Destroy giáp đang mặc → gỡ trang bị");
                    eq.RemoveArmor(armorData.armorSlot);
                }
            }

            PlayerInventory.Instance.items[currentSlotIndex] = null;
            PlayerInventory.Instance.RaiseInventoryChanged("Destroy item từ đúng slot");

            Close();
        });

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
        EventSystem.current.SetSelectedGameObject(null);
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
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                bool clickedInsideButton = false;
                foreach (var r in results)
                {
                    if (r.gameObject.transform.IsChildOf(transform))
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

    private System.Collections.IEnumerator EnableInteractionNextFrame()
    {
        yield return null;
        dropButton.interactable = true;
        dropAmountButton.interactable = true;
        destroyButton.interactable = true;
        Debug.Log("[Popup] Buttons đã được bật tương tác.");
    }
}
