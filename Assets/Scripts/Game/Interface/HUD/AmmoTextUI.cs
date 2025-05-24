using TMPro;
using UnityEngine;

public class AmmoTextUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI clipText;
    [SerializeField] private PlayerInventory inventory;

    private WeaponRuntimeItem runtime;

    public void Bind(WeaponRuntimeItem runtimeData)
    {
        Debug.Log($"[AmmoTextUI] Bind được gọi → {runtimeData?.baseData?.itemID ?? "null"}");

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

    private void OnDisable()
    {
        if (runtime != null)
            runtime.OnAmmoChanged -= Refresh;
    }

    public void Refresh()
    {
        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
            return;
        }

        int reserve = inventory.GetAmmoCount(runtime.currentAmmoType);
        clipText.text = $"{runtime.ammoInClip:00} / {reserve:00}";
    }
}
