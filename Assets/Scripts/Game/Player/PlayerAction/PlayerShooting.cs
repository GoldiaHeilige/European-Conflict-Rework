using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public PlayerReload playerReload;

    private WeaponRuntimeData weaponRuntime;
    private float nextFireTime = 0f;

    public void SetWeapon(WeaponRuntimeData runtime)
    {
        weaponRuntime = runtime;
        Debug.Log($"[PlayerShooting] Gắn weapon: {runtime.data.weaponName}");
    }

    private void Update()
    {
        if (weaponRuntime == null || weaponRuntime.data == null || firePoint == null) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (weaponRuntime.CanFire() && (playerReload == null || !playerReload.IsReloading))
            {
                Shoot();
                nextFireTime = Time.time + weaponRuntime.data.fireRate;
            }
        }
    }

    private void Shoot()
    {
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
