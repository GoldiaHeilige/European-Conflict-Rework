using TMPro;
using UnityEngine;

public class AmmoTypeTextUI : MonoBehaviour
{
    public TMP_Text ammoTypeText;

    private void OnEnable()
    {
        UpdateAmmoType();

        if (PlayerWeaponCtrl.Instance?.runtimeItem != null)
        {
            PlayerWeaponCtrl.Instance.runtimeItem.OnAmmoChanged += UpdateAmmoType;
        }
    }

    private void OnDisable()
    {
        if (PlayerWeaponCtrl.Instance?.runtimeItem != null)
        {
            PlayerWeaponCtrl.Instance.runtimeItem.OnAmmoChanged -= UpdateAmmoType;
        }
    }

    private void Update()
    {
        // Trường hợp đổi súng, cần theo dõi lại
        if (PlayerWeaponCtrl.Instance?.runtimeItem != null)
        {
            PlayerWeaponCtrl.Instance.runtimeItem.OnAmmoChanged -= UpdateAmmoType;
            PlayerWeaponCtrl.Instance.runtimeItem.OnAmmoChanged += UpdateAmmoType;
        }

        UpdateAmmoType(); // cập nhật mỗi frame để đảm bảo đúng súng
    }

    public void UpdateAmmoType()
    {
        var runtime = PlayerWeaponCtrl.Instance?.runtimeItem;
        if (runtime == null || runtime.currentAmmoType == null)
        {
            ammoTypeText.text = "--- (-)";
            return;
        }

        var ammo = runtime.currentAmmoType;
        ammoTypeText.text = $"{ammo.shortName}  (T {ammo.tier})";
    }
}
