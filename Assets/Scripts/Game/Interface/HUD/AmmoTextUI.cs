using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoTextUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI clipText;
    [SerializeField] private PlayerWeaponCtrl weaponCtrl;
    [SerializeField] private PlayerInventory inventory;

    private WeaponRuntimeData runtime;

    public void Bind(WeaponRuntimeData runtimeData)
    {
        runtime = runtimeData;
    }

    private void Update()
    {
        if (runtime == null || runtime.currentAmmoType == null)
        {
            clipText.text = "-- / --";
            Debug.LogWarning("[HUD] runtime hoặc currentAmmoType NULL");
            return;
        }

        int reserve = inventory.GetAmmoCount(runtime.currentAmmoType);
        clipText.text = $"{runtime.ammoInClip} / {reserve}";
        Debug.Log($"[HUD] currentAmmoType = {runtime.currentAmmoType.name}, ammoName = {runtime.currentAmmoType.ammoName}");

    }
}
