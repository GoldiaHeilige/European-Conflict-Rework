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

        Vector2 dir = (player.position - firePoint.position).normalized;

        GameObject bulletObj = Instantiate(weaponRuntime.data.bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet_Kinetic bullet = bulletObj.GetComponent<Bullet_Kinetic>();

        if (bullet != null)
        {
            bullet.Initialize(dir, weaponRuntime.data.bulletSpeed, 2f);
            bullet.SetOwnerAndDamage(this.gameObject, weaponRuntime.data.damage);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f);
        }

        weaponRuntime.ConsumeBullet();
    }
}
