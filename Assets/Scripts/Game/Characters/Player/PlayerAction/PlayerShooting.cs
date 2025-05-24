using UnityEngine;

public class PlayerShooting : WpnShootingBase
{
    [SerializeField] private PlayerReload playerReload;
    [SerializeField] private BulletPoolManager bulletPoolManager;
    [SerializeField] private PlayerInventory playerInventory;

    public override void SetWeapon(WeaponRuntimeItem runtime)
    {
        weaponRuntime = runtime;
    }

    protected override bool ShouldShoot()
    {
        if (playerReload != null && playerReload.IsReloading) return false;
        if (Time.timeScale == 0) return false;
        return Input.GetButton("Fire1");
    }

    protected override void Shoot()
    {
        if (weaponRuntime == null || weaponRuntime.baseData == null || firePoint == null) return;
        if (!weaponRuntime.CanFire()) return;

        AmmoData ammo = weaponRuntime.currentAmmoType;
        if (ammo == null || ammo.bulletPrefab == null) return;

        ObjectPool pool = bulletPoolManager.GetPoolForPrefab(ammo.bulletPrefab);
        if (pool == null) return;

        GameObject bulletObj = pool.Get();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 fireDir = (mouseWorldPos - (Vector2)firePoint.position).normalized;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg + 90f);

        BulletCtrl bullet = bulletObj.GetComponent<BulletCtrl>();
        if (bullet != null)
        {
            bullet.SetAmmoInfo(this.gameObject, ammo);
            bullet.SetBulletTag("PlayerBullet");
            bullet.SetLayer(LayerMask.NameToLayer("PlayerBullet"));
            bullet.Initialize(fireDir, ammo.bulletSpeed, 2f);
        }

        AutoReturnToPool auto = bulletObj.GetComponent<AutoReturnToPool>();
        if (auto != null)
        {
            auto.Init(pool);
        }

        weaponRuntime.ConsumeBullet();
PlayerWeaponCtrl.Instance?.runtimeItem?.OnAmmoChanged?.Invoke();
    }
}
