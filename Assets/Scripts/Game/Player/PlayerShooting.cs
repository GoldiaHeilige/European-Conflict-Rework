using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public TextMeshProUGUI ammoText;

    private WeaponRuntimeData weaponRuntime;
    private float nextFireTime = 0f;

    public void SetWeapon(WeaponRuntimeData runtime)
    {
        weaponRuntime = runtime;
    }

    private void Update()
    {
        if (weaponRuntime == null || firePoint == null || weaponRuntime.data == null) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (weaponRuntime.CanFire())
            {
                Shoot();
                nextFireTime = Time.time + weaponRuntime.data.fireRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && weaponRuntime.CanReload())
        {
            weaponRuntime.Reload();
        }

        if (ammoText != null)
        {
            ammoText.text = $"{weaponRuntime.currentAmmo} / {weaponRuntime.currentReserve}";
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(weaponRuntime.data.bulletPrefab, firePoint.position, Quaternion.identity);
        BulletCtrl bullet = bulletObj.GetComponent<BulletCtrl>();
        if (bullet != null)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDir = (mouseWorldPos - (Vector2)firePoint.position).normalized;

            bullet.Initialize(fireDir, weaponRuntime.data.bulletSpeed, 2f);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg + 90f);
        }

        weaponRuntime.ConsumeBullet(); 
    }
}
