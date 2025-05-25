using DG.Tweening;
using TMPro;
using UnityEngine;

public class AmmoTextUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI clipText;
    [SerializeField] private PlayerInventory inventory;

    [Header("Popup Effect")]
    [SerializeField] private Transform scaleTarget;
    [SerializeField] private Vector2 popupIntensity = new Vector2(0.2f, 0.2f);
    [SerializeField] private float popupDuration = 0.2f;

    private string lastDisplayText = "";

    private WeaponRuntimeItem runtime;

    public void Bind(WeaponRuntimeItem runtimeData)
    {

        // Hủy đăng ký vũ khí cũ nếu có
        if (runtime != null)
            runtime.OnAmmoChanged -= Refresh;

        runtime = runtimeData;

        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
            return;
        }

        runtime.OnAmmoChanged += Refresh;
        Refresh();
    }

    private void OnEnable()
    {
        PlayerInventory.InventoryChanged += OnInventoryChanged;
    }

    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= OnInventoryChanged;
        if (runtime != null)
            runtime.OnAmmoChanged -= Refresh;
    }


    private void OnInventoryChanged()
    {
        Refresh();
    }


    public void Refresh()
    {
        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
            lastDisplayText = "-- / --";
            return;
        }

        int reserve = inventory.GetAmmoCount(runtime.currentAmmoType);
        string newText = $"{runtime.ammoInClip:00} / {reserve:00}";

        // ❌ Nếu giống lần trước → không làm gì
        if (newText == lastDisplayText) return;

        clipText.text = newText;
        lastDisplayText = newText;

        // ✅ Animation chỉ chạy khi text thực sự thay đổi
        if (scaleTarget != null)
        {
            scaleTarget.DOKill();
            scaleTarget.localScale = Vector3.one;

            scaleTarget
                .DOPunchScale(popupIntensity, popupDuration)
                .OnComplete(() => scaleTarget.DORewind());
        }
    }
}
