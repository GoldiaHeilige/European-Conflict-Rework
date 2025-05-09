
using UnityEngine;

public class EnemyShooting : WpnShootingBase
{
    [SerializeField] private BulletPoolManager bulletPoolManager;

    private Transform player;
    public bool shouldShoot = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override bool ShouldShoot()
    {
        return shouldShoot && player != null;
    }

    protected override void Shoot()
    {
        if (weaponRuntime == null || weaponRuntime.data == null || firePoint == null) return;

        Vector2 dir = new Vector2(Mathf.Cos((firePoint.eulerAngles.z - 90) * Mathf.Deg2Rad),
                                  Mathf.Sin((firePoint.eulerAngles.z - 90) * Mathf.Deg2Rad)).normalized;

        GameObject prefab = weaponRuntime.data.bulletPrefab;
        ObjectPool pool = bulletPoolManager.GetPoolForPrefab(prefab);
        if (pool == null) return;

        GameObject bulletObj = pool.Get();
        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f);

        BulletCtrl baseBullet = bulletObj.GetComponent<BulletCtrl>();
        if (baseBullet != null)
        {
            baseBullet.SetBulletType(weaponRuntime.data.bulletType);
            baseBullet.SetOwnerAndDamage(this.gameObject, weaponRuntime.data.damage);
            baseBullet.SetBulletTag("EnemyBullet");
            baseBullet.SetLayer(LayerMask.NameToLayer("EnemyBullet"));
            baseBullet.Initialize(dir, weaponRuntime.data.bulletSpeed, 2f);
        }

        AutoReturnToPool auto = bulletObj.GetComponent<AutoReturnToPool>();
        if (auto != null)
        {
            auto.Init(pool);
        }

        weaponRuntime.ConsumeBullet();
    }
}
