
using UnityEngine;
using System;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("References")]
    public PlayerWeaponCtrl weaponController;
    public PlayerInventory inventory;
    public PlayerReload reloadController;

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
            PlayerWeaponCtrl.Instance?.ClearWeapon();
            return;
        }

        // ✅ Nếu đang reload thì cancel (súng cũ)
        if (PlayerWeaponCtrl.Instance?.playerReload != null &&
            PlayerWeaponCtrl.Instance.playerReload.IsReloading)
        {
            PlayerWeaponCtrl.Instance.playerReload.CancelReload();
        }

        // ✅ Phát âm chamber sound của súng mới
        if (updatedWeapon.baseData is WeaponData weaponData)
        {
            string prefix = weaponData.weaponPrefix;
            AudioManager.Instance.StopByTag("Reload"); // dừng reload sound còn sót
            AudioManager.Instance.Play("Chamber", $"{prefix}_Chamber");
            Debug.Log($"[Chamber Sound] GỌI → Prefix = {prefix}");
        }

        // ✅ Gán weapon mới
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
