using UnityEngine;

public class EnemyWeaponCtrl : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponData weaponData;

    [Header("References")]
    public EnemyShooting enemyShooting;
    public EnemyReload enemyReload;

    private WeaponRuntimeData runtimeData;

    private void Start()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("EnemyWeaponCtrl: weaponData bị thiếu!");
            return;
        }

        runtimeData = new WeaponRuntimeData(weaponData);

        // Đạn vô hạn
        runtimeData.currentReserve = 9999;

        if (enemyShooting != null)
            enemyShooting.SetWeapon(runtimeData);

        if (enemyReload != null)
            enemyReload.SetWeapon(runtimeData);
    }

    public WeaponRuntimeData GetRuntimeData()
    {
        return runtimeData;
    }
}
