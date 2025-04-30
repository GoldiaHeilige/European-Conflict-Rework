using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    private WeaponData currentWeaponData;
    private float nextFireTime = 0f;

    public void SetWeapon(WeaponData weapon)
    {
        currentWeaponData = weapon;
    }

    private void Update()
    {
        if (currentWeaponData == null || firePoint == null) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentWeaponData.fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(currentWeaponData.bulletPrefab, firePoint.position, Quaternion.identity);
        BulletCtrl bullet = bulletObj.GetComponent<BulletCtrl>();
        if (bullet != null)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDir = (mouseWorldPos - (Vector2)firePoint.position).normalized;

            bullet.Initialize(fireDir, currentWeaponData.bulletSpeed, 2f);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg + 90f);
        }
    }
}
