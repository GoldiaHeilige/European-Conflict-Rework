using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeAmmoTypePopup : MonoBehaviour
{
    public Transform contentRoot;
    public GameObject ammoButtonPrefab;
    public Button closeButton;

    public static ChangeAmmoTypePopup Instance;

    public static void InitInstanceIfNeeded()
    {
        if (Instance != null) return;

        Instance = FindObjectOfType<ChangeAmmoTypePopup>(true);
        if (Instance == null)
        {
            Debug.LogError("[ChangeAmmoPopup] Không tìm thấy instance trong scene!");
        }
        else
        {
            Debug.Log("[ChangeAmmoPopup] Instance được tìm thấy thủ công qua FindObjectOfType");
        }
    }


    public void Show(WeaponRuntimeItem weapon)
    {
        if (weapon == null || weapon.baseData == null)
        {
            Debug.LogError("[ChangeAmmoPopup] Weapon hoặc baseData null");
            return;
        }

        if (contentRoot == null || ammoButtonPrefab == null)
        {
            Debug.LogError("[ChangeAmmoPopup] contentRoot hoặc ammoButtonPrefab chưa được gán trong Inspector");
            return;
        }

        Debug.Log($"[ChangeAmmoPopup] Đang hiển thị popup cho súng: {weapon.baseData.itemID} (class: {weapon.baseData.weaponClass})");

        if (PlayerInventory.Instance == null || PlayerInventory.Instance.knownAmmoTypes == null)
        {
            Debug.LogError("[ChangeAmmoPopup] PlayerInventory hoặc knownAmmoTypes null");
            return;
        }

        Debug.Log($"[ChangeAmmoPopup] knownAmmoTypes count = {PlayerInventory.Instance.knownAmmoTypes.Count}");

        ClearUI();

        foreach (var ammo in PlayerInventory.Instance.knownAmmoTypes)
        {
            if (ammo == null)
            {
                Debug.LogWarning("[ChangeAmmoPopup] Có 1 ammo null trong danh sách");
                continue;
            }

            Debug.Log($"[AmmoCheck] {ammo.ammoName} | compatibleWeapon = {ammo.compatibleWeapon} vs súng = {weapon.baseData.weaponClass}");

            if (ammo.compatibleWeapon != weapon.baseData.weaponClass)
            {
                Debug.Log($"[AmmoCheck] → BỎ QUA {ammo.ammoName} vì không tương thích");
                continue;
            }

            int count = PlayerInventory.Instance.GetAmmoCount(ammo);
            Debug.Log($"[AmmoCheck] → {ammo.ammoName} có {count} viên trong kho");

            if (count <= 0)
            {
                Debug.Log($"[AmmoCheck] → BỎ QUA {ammo.ammoName} vì hết đạn");
                continue;
            }

            Debug.Log($"[AmmoCheck] → Hiển thị: {ammo.ammoName} ({count})");

            var btn = Instantiate(ammoButtonPrefab, contentRoot);
            btn.GetComponentInChildren<TMPro.TMP_Text>().text = $"{ammo.ammoName} ({count})";

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerWeaponCtrl.Instance.pendingAmmoChange = ammo;
                PlayerWeaponCtrl.Instance.ApplyPendingAmmoChange();
                Debug.Log($"[ChangeAmmoPopup] Chọn đạn: {ammo.ammoName}");
                gameObject.SetActive(false);

                PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
                PlayerInventory.Instance?.RaiseInventoryChanged("ChangeAmmoTypePopup -> Reload UI");
            });
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                UIStackClose.Remove(this.gameObject);
                gameObject.SetActive(false);
            });
        }


        gameObject.SetActive(true);
        UIStackClose.Push(this.gameObject);
        Debug.Log("[ChangeAmmoPopup] Popup hiển thị thành công");
    }

    private void ClearUI()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);
    }
}
