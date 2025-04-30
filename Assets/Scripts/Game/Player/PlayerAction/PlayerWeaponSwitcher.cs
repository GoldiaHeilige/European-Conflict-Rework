using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("Vũ khí")]
    public WeaponData[] weaponList;
    private int currentWeaponIndex = 0;

    [Header("Tham chiếu")]
    public PlayerWeaponHandler weaponHandler;

    private void Start()
    {
/*        if (weaponList.Length > 0 && weaponHandler != null)
        {
            weaponHandler.EquipWeapon(weaponList[currentWeaponIndex]);
        }*/
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
            weaponHandler.EquipWeapon(weaponList[index]);
            Debug.Log($"Weapon changed to: {weaponList[index].weaponName}");
        }
    }
}
