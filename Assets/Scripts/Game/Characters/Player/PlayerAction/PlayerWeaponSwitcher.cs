using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponData[] weaponList;
    private int currentWeaponIndex = 0;

    [Header("References")]
    public PlayerWeaponCtrl weaponController;

    public PlayerReload playerReload; // ✅ Thêm tham chiếu để huỷ reload

    public static event Action<WeaponData> OnWeaponSwitched;

    private Dictionary<WeaponData, WeaponRuntimeData> runtimeDataMap = new();

    private void Start()
    {
        if (weaponList.Length > 0 && weaponController != null)
        {
            EquipCurrentWeapon();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weaponList.Length > 1)
        {
            SwitchWeapon(1);
        }
    }

    private void SwitchWeapon(int index)
    {
        if (index < weaponList.Length && weaponList[index] != null && weaponController != null)
        {
            if (playerReload != null)
            {
                playerReload.CancelReload();
            }

            currentWeaponIndex = index;
            EquipCurrentWeapon();
        }
    }

    private void EquipCurrentWeapon()
    {
        var weapon = weaponList[currentWeaponIndex];

        if (!runtimeDataMap.ContainsKey(weapon))
        {
            AmmoData defaultAmmo = PlayerInventory.Instance?.GetDefaultAmmoFor(weapon.weaponClass);
            runtimeDataMap[weapon] = new WeaponRuntimeData(weapon, defaultAmmo);
        }

        WeaponRuntimeData runtime = runtimeDataMap[weapon];
        weaponController.EquipWeapon(runtime);
        OnWeaponSwitched?.Invoke(weapon);
    }

}
