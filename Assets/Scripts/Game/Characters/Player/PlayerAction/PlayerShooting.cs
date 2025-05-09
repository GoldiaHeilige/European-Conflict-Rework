
using UnityEngine;
using static WeaponData;

public class PlayerShooting : WpnShootingBase
{
    [SerializeField] private PlayerReload playerReload;
    [SerializeField] private BulletPoolManager bulletPoolManager;

    protected override bool ShouldShoot()
    {
        if (playerReload != null && playerReload.IsReloading) return false;

        if (Time.timeScale == 0) return false;

        return Input.GetButton("Fire1");
    }

    protected override void Shoot()
    {
        if (weaponRuntime == null || weaponRuntime.data == null || firePoint == null) return;

        GameObject prefab = weaponRuntime.data.bulletPrefab;
        ObjectPool pool = bulletPoolManager.GetPoolForPrefab(prefab);
        if (pool == null) return;

        GameObject bulletObj = pool.Get();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 fireDir = (mouseWorldPos - (Vector2)firePoint.position).normalized;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg + 90f);

        BulletCtrl baseBullet = bulletObj.GetComponent<BulletCtrl>();
        if (baseBullet != null)
        {
            baseBullet.SetBulletType(weaponRuntime.data.bulletType);
            baseBullet.SetOwnerAndDamage(this.gameObject, weaponRuntime.data.damage);
            baseBullet.SetBulletTag("PlayerBullet");
            baseBullet.SetLayer(LayerMask.NameToLayer("PlayerBullet"));
            baseBullet.Initialize(fireDir, weaponRuntime.data.bulletSpeed, 2f);
        }

        AutoReturnToPool auto = bulletObj.GetComponent<AutoReturnToPool>();
        if (auto != null)
        {
            auto.Init(pool);
        }

        weaponRuntime.ConsumeBullet();
    }
}
