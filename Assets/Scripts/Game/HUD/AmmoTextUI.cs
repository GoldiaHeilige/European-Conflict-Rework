using TMPro;
using UnityEngine;

public class AmmoTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private WeaponRuntimeData runtimeData;

    public void Bind(WeaponRuntimeData data)
    {
        if (runtimeData != null)
        {
            runtimeData.OnAmmoChanged -= UpdateUI;
        }

        runtimeData = data;
        runtimeData.OnAmmoChanged += UpdateUI;

        UpdateUI();
    }

    private void OnDisable()
    {
        if (runtimeData != null)
        {
            runtimeData.OnAmmoChanged -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (ammoText != null && runtimeData != null)
        {
            ammoText.text = $"{runtimeData.currentAmmo} / {runtimeData.currentReserve}";
        }
    }
}
