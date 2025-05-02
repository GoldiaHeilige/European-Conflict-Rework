using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponData[] weaponList;
    private int currentWeaponIndex = 0;

    [Header("References")]
    public PlayerWeaponHandler weaponHandler;

    public static event Action<WeaponData> OnWeaponSwitched;
    private Dictionary<WeaponData, WeaponRuntimeData> runtimeDataMap = new();

    private void Start()
    {
        if (weaponList.Length > 0 && weaponHandler != null)
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
        if (index < weaponList.Length && weaponList[index] != null && weaponHandler != null)
        {
            currentWeaponIndex = index;
            EquipCurrentWeapon();
        }
    }

    private void EquipCurrentWeapon()
    {
        var weapon = weaponList[currentWeaponIndex];

        if (!runtimeDataMap.ContainsKey(weapon))
            runtimeDataMap[weapon] = new WeaponRuntimeData(weapon);

        WeaponRuntimeData runtime = runtimeDataMap[weapon];
        weaponHandler.EquipWeapon(runtimeDataMap[weapon]);

        OnWeaponSwitched?.Invoke(weapon);
    }
}
