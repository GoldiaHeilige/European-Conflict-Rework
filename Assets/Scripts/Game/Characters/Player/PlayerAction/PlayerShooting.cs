using UnityEngine;

public class PlayerShooting : WpnShootingBase
{
    [SerializeField] private PlayerReload playerReload;

    protected override bool ShouldShoot()
    {
        if (playerReload != null && playerReload.IsReloading) return false;

        return Input.GetButton("Fire1");
    }

    protected override void Shoot()
    {
        if (weaponRuntime == null || weaponRuntime.data == null || firePoint == null) return;

        GameObject bulletObj = Instantiate(weaponRuntime.data.bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet_Kinetic bullet = bulletObj.GetComponent<Bullet_Kinetic>();

        if (bullet != null)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDir = (mouseWorldPos - (Vector2)firePoint.position).normalized;

            bullet.Initialize(fireDir, weaponRuntime.data.bulletSpeed, 2f);
            bullet.SetOwnerAndDamage(this.gameObject, weaponRuntime.data.damage);

            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg + 90f);
        }

        weaponRuntime.ConsumeBullet();
    }
}
