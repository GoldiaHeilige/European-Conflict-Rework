using TMPro;
using UnityEngine;

public class AmmoTypeTextUI_Slot : MonoBehaviour
{
    public TMP_Text ammoTypeText;
    private WeaponRuntimeItem runtime;

    public void Bind(WeaponRuntimeItem runtimeData)
    {
        if (runtime != null)
            runtime.OnAmmoChanged -= UpdateAmmoType;

        runtime = runtimeData;

        if (runtime == null || runtime.currentAmmoType == null)
        {
            ammoTypeText.text = "--- (-)";
            return;
        }

        runtime.OnAmmoChanged += UpdateAmmoType;
        UpdateAmmoType();
    }

    private void OnDisable()
    {
        if (runtime != null)
            runtime.OnAmmoChanged -= UpdateAmmoType;
    }

    public void UpdateAmmoType()
    {
        if (runtime == null || runtime.currentAmmoType == null)
        {
            ammoTypeText.text = "--- (-)";
            return;
        }

        var ammo = runtime.currentAmmoType;
        ammoTypeText.text = $"{ammo.shortName}  (T {ammo.tier})";
    }
}
