using UnityEngine;

public class EnemyGunCtrl : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponData weaponData;

    [Header("FirePoint")]
    [SerializeField] public Transform firePoint; // gán thủ công trong Inspector

    private float fireCooldown;
    private int currentClip;
    private bool isReloading;
    private float reloadTimer;

    private void Start()
    {
        currentClip = weaponData.clipSize;
        isReloading = false;
        fireCooldown = 0f;
    }

    private void Update()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                currentClip = weaponData.clipSize;
                isReloading = false;
                Debug.Log("🔁 Enemy đã reload xong");
            }
        }

        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;
    }

    public bool CanShoot()
    {
        return !isReloading && fireCooldown <= 0f && currentClip >= weaponData.ammoPerShot;
    }

    public void Reload()
    {
        if (isReloading) return;

        isReloading = true;
        reloadTimer = weaponData.reloadTime;
        Debug.Log("⏳ Enemy bắt đầu reload...");
    }

    public void TryShoot(Vector2 direction, GameObject owner)
    {
        if (!CanShoot()) return;

        fireCooldown = 1f / weaponData.fireRate;
        currentClip -= weaponData.ammoPerShot;

        GameObject bulletObj = Instantiate(weaponData.bulletPrefab, firePoint.position, Quaternion.identity);

        Bullet_Kinetic bullet = bulletObj.GetComponent<Bullet_Kinetic>();
        if (bullet != null)
        {
            bullet.Initialize(direction, weaponData.bulletSpeed, 2f);
            bullet.SetOwnerAndDamage(owner, weaponData.damage);


            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f);
        }

        Debug.Log($"Enemy bắn → clip: {currentClip}/{weaponData.clipSize}");
    }

    public bool IsReloading() => isReloading;
    public bool IsClipEmpty() => currentClip <= 0;
}
