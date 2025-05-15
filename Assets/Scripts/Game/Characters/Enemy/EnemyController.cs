using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyWeaponCtrl weaponCtrl;

    private void Start()
    {
        if (weaponCtrl != null)
            weaponCtrl.EquipDefaultWeapon();
    }
}
