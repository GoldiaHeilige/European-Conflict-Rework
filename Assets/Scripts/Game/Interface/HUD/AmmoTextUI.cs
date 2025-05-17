using TMPro;
using UnityEngine;

public class AmmoTextUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI clipText;
    [SerializeField] private PlayerWeaponCtrl weaponCtrl;
    [SerializeField] private PlayerInventory inventory;

    private WeaponRuntimeItem runtime;

    public void Bind(WeaponRuntimeItem runtimeData)
    {
        runtime = runtimeData;
    }

    private void Update()
    {
        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
            return;
        }

        int reserve = inventory.GetAmmoCount(runtime.currentAmmoType);
        clipText.text = $"{runtime.ammoInClip} / {reserve}";
    }
}
