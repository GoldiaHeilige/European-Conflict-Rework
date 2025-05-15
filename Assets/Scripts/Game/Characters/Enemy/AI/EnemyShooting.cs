using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    private WeaponRuntimeData runtime;

    public void SetWeapon(WeaponRuntimeData data)
    {
        runtime = data;
    }

    public void TryShootAt(Vector3 targetPosition)
    {
        if (runtime == null || !runtime.CanFire()) return;

        AmmoData ammo = runtime.currentAmmoType;
        if (ammo == null || ammo.bulletPrefab == null) return;

        GameObject bulletObj = Instantiate(ammo.bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 dir = ((Vector2)(targetPosition - firePoint.position)).normalized;

        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f);

        BulletCtrl bullet = bulletObj.GetComponent<BulletCtrl>();
        if (bullet != null)
        {
            bullet.SetAmmoInfo(gameObject, ammo);
            bullet.SetBulletTag("EnemyBullet");
            bullet.SetLayer(LayerMask.NameToLayer("EnemyBullet"));
            bullet.Initialize(dir, ammo.baseDamage, 2f);
        }

        runtime.ConsumeBullet();
    }
}
