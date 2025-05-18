using UnityEngine;
using System;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("References")]
    public PlayerWeaponCtrl weaponController;
    public PlayerInventory inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(1);
        else if (Input.GetKeyDown(KeyCode.Q)) ToggleWeapon();
    }

    private void Equip(int index)
    {
        if (index < 0 || index >= inventory.weaponSlots.Length) return;

        var weapon = inventory.weaponSlots[index];
        if (weapon == null || weapon.baseData == null)
        {
            Debug.LogWarning($"[WeaponSwitcher] Equip slot {index} bị null hoặc thiếu baseData");
            return;
        }

        inventory.EquipWeaponSlot(index);
        weaponController.EquipWeapon(weapon);
    }


    private void ToggleWeapon()
    {
        int otherIndex = inventory.currentWeaponIndex == 0 ? 1 : 0;
        var other = inventory.weaponSlots[otherIndex];
        if (other != null)
        {
            Equip(otherIndex);
        }
    }
}
