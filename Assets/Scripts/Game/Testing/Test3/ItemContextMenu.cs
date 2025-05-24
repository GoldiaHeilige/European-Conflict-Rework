using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ItemContextMenu : MonoBehaviour
{
    public Button inspectButton;
    public Button dropButton;
    public Button dropAmountButton;
    public Button destroyButton;
    public Button changeAmmoButton;
    public Button healingButton;
    public Button closeButton;

    private InventoryItemRuntime currentItem;
    private RectTransform rectTransform;
    private int currentSlotIndex = -1;

    public static ItemContextMenu currentOpenMenu;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Click vào UI");
            }
            else
            {
                Debug.Log("Click ra ngoài UI");
            }
        }
    }


    public void Setup(InventoryItemRuntime item, int slotIndex, Vector2 screenPosition, Canvas canvas)
    {
        currentItem = item;
        currentSlotIndex = slotIndex;

        if (currentOpenMenu != null && currentOpenMenu != this)
            Destroy(currentOpenMenu.gameObject);
        currentOpenMenu = this;

        // Reset all buttons
        dropButton.gameObject.SetActive(true);
        dropAmountButton.gameObject.SetActive(
            item.itemData is AmmoItemData ||
            (item.itemData is HealingItemData heal && heal.stackable)
        );

        destroyButton.gameObject.SetActive(true);
        changeAmmoButton.gameObject.SetActive(false); // Tạm ẩn, chỉ hiện nếu là súng đang cầm

        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(() => {
            DropItem();
            Close();
        });

        dropAmountButton.onClick.RemoveAllListeners();
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
            DestroyItem();
            Close();
        });

        inspectButton.gameObject.SetActive(true);
        inspectButton.onClick.RemoveAllListeners();
        inspectButton.onClick.AddListener(() => {
            ItemInspectUI.InitIfNeeded();
            if (ItemInspectUI.Instance != null)
            {
                ItemInspectUI.Instance.Show(currentItem);
            }
            else
            {
                Debug.LogError("Không thể hiển thị inspect, Instance bị null.");
            }
            Close();
        });



        // 🔥 Nếu là súng đang được cầm → cho phép đổi đạn
        if (currentItem is WeaponRuntimeItem weapon)
        {
            var equipped = PlayerWeaponCtrl.Instance?.runtimeItem;
            if (equipped != null && equipped.runtimeId == weapon.runtimeId)
            {
                changeAmmoButton.gameObject.SetActive(true);
                changeAmmoButton.onClick.RemoveAllListeners();
                changeAmmoButton.onClick.AddListener(() => {
                    Debug.Log($"[CTX] ChangeAmmoTypePopup.Instance = {(ChangeAmmoTypePopup.Instance == null ? "NULL" : "OK")}");

                    ChangeAmmoTypePopup.InitInstanceIfNeeded();
                    var popup = FindObjectOfType<ChangeAmmoTypePopup>(true);
                    if (popup != null)
                    {
                        popup.Show(equipped);
                    }
                    else
                    {
                        Debug.LogError("[CTX] Không tìm thấy ChangeAmmoTypePopup trong scene!");
                    }
                    Close();
                });
            }
        }

        healingButton.gameObject.SetActive(false);

        // ✅ Chỉ hiện nếu là item hồi máu
        if (item.itemData is HealingItemData)
        {
            healingButton.gameObject.SetActive(true);
            healingButton.onClick.RemoveAllListeners();
            healingButton.onClick.AddListener(() => {
                Debug.Log("🟩 Đã nhấn nút Use!");
                HealingSystem.Instance?.HealFromItem(item);
                Close();
            });
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close); // gọi hàm Close có sẵn
        }

        // Đặt vị trí
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

    private void DropItem()
    {
        if (currentItem is WeaponRuntimeItem wpn)
        {
            if (PlayerInventory.Instance.equippedWeapon != null &&
                PlayerInventory.Instance.equippedWeapon.runtimeId == currentItem.runtimeId)
            {
                PlayerInventory.Instance.UnequipWeapon();
            }

            for (int i = 0; i < PlayerInventory.Instance.weaponSlots.Length; i++)
            {
                var slotWpn = PlayerInventory.Instance.weaponSlots[i];
                if (slotWpn != null && slotWpn.runtimeId == currentItem.runtimeId)
                {
                    PlayerInventory.Instance.UnequipWeaponSlot(i);
                }
            }
        }

        if (currentItem.itemData is ArmorData armorData)
        {
            var eq = FindObjectOfType<EquippedArmorManager>();
            var equipped = eq?.GetArmor(armorData.armorSlot);
            if (equipped != null && equipped.sourceItem.runtimeId == currentItem.runtimeId)
            {
                eq.RemoveArmor(armorData.armorSlot);
            }
        }

        DropSpawner.Instance.Spawn(currentItem, true);
        PlayerInventory.Instance.RemoveExactItem(currentItem);
        PlayerInventory.Instance.RaiseInventoryChanged("Drop item từ đúng slot");

        // ✅ Trừ khỏi ammoCounts nếu là AmmoItemData
        if (currentItem.itemData is AmmoItemData ammoItemData)
        {
            var matched = PlayerInventory.Instance.knownAmmoTypes
                .FirstOrDefault(a => a.ammoName == ammoItemData.linkedAmmoData.ammoName);

            if (matched != null)
            {
                int before = PlayerInventory.Instance.GetAmmoCount(matched);
                int amount = currentItem.quantity;
                PlayerInventory.Instance.ForceSetAmmoCount(matched, before - amount);

                Debug.Log($"[DropContextMenu] Trừ {amount} viên {matched.ammoName} khỏi ammoCounts → từ {before} còn {before - amount}");
                PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
            }
        }

    }

    private void DestroyItem()
    {
        if (currentItem is WeaponRuntimeItem wpn &&
            PlayerInventory.Instance.equippedWeapon != null &&
            PlayerInventory.Instance.equippedWeapon.runtimeId == currentItem.runtimeId)
        {
            PlayerInventory.Instance.UnequipWeapon();
            PlayerWeaponCtrl.Instance?.ClearWeapon();
        }

        if (currentItem.itemData is ArmorData armorData)
        {
            var eq = FindObjectOfType<EquippedArmorManager>();
            var equipped = eq?.GetArmor(armorData.armorSlot);
            if (equipped != null && equipped.sourceItem.runtimeId == currentItem.runtimeId)
            {
                eq.RemoveArmor(armorData.armorSlot);
            }
        }

        PlayerInventory.Instance.items[currentSlotIndex] = null;
        PlayerInventory.Instance.RaiseInventoryChanged("Destroy item từ đúng slot");
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
        if (changeAmmoButton != null)
            changeAmmoButton.interactable = true;

        Debug.Log("[Popup] Buttons đã được bật tương tác.");
    }
}
