using UnityEngine;

public class EnemyShooting : WpnShootingBase
{
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

        GameObject bulletObj = Instantiate(weaponRuntime.data.bulletPrefab, firePoint.position, Quaternion.identity);
        BulletCtrl baseBullet = bulletObj.GetComponent<BulletCtrl>();

        if (baseBullet != null)
        {
            baseBullet.Initialize(dir, weaponRuntime.data.bulletSpeed, 2f);
            baseBullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90);
        }

        Bullet_Kinetic kinetic = bulletObj.GetComponent<Bullet_Kinetic>();
        if (kinetic != null)
        {
            kinetic.SetOwnerAndDamage(this.gameObject, weaponRuntime.data.damage);
        }

        Bullet_Explosive explosive = bulletObj.GetComponent<Bullet_Explosive>();
        if (explosive != null)
        {
            explosive.SetOwnerAndDamage(this.gameObject, weaponRuntime.data.damage);
        }

        weaponRuntime.ConsumeBullet();
    }

}
