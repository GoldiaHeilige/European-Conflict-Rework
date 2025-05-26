
using UnityEngine;
using System;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("References")]
    public PlayerWeaponCtrl weaponController;
    public PlayerInventory inventory;

    private void Update()
    {
        if (PauseMenu.IsGamePaused || InterfaceManager.IsInventoryOpen) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(1);
        else if (Input.GetKeyDown(KeyCode.Q)) ToggleWeapon();
    }


    private void Equip(int index)
    {
        if (index < 0 || index >= inventory.weaponSlots.Length) return;

        inventory.EquipWeaponSlot(index);

        var updatedWeapon = inventory.weaponSlots[index];
        if (updatedWeapon == null || updatedWeapon.baseData == null)
        {
            weaponController.ClearWeapon();
            if (PlayerWeaponCtrl.Instance != null)
            {
/*                Debug.LogWarning($"[PlayerWeaponSwitcher] GỌI ClearWeapon() → Ctrl ID từ Equip = {PlayerWeaponCtrl.Instance.GetInstanceID()}");*/
                PlayerWeaponCtrl.Instance.ClearWeapon();
            }
            else
            {
                Debug.LogError("[PlayerWeaponSwitcher] PlayerWeaponCtrl.Instance == null → KHÔNG GỌI ĐƯỢC");
            }

            return;
        }

        PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();      
        weaponController.EquipWeapon(updatedWeapon);
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
