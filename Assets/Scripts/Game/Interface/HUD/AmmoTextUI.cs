using TMPro;
using UnityEngine;

public class AmmoTextUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI clipText;
    [SerializeField] private PlayerInventory inventory;

    private WeaponRuntimeItem runtime;

    public void Bind(WeaponRuntimeItem runtimeData)
    {
        runtime = runtimeData;

        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
        }
        else
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
            return;
        }

        int reserve = inventory.GetAmmoCount(runtime.currentAmmoType);
        clipText.text = $"{runtime.ammoInClip:00} / {reserve}";
    }
}
