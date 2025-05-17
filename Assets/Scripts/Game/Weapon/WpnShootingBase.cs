using UnityEngine;

public abstract class WpnShootingBase : MonoBehaviour
{
    protected WeaponRuntimeItem weaponRuntime;
    [SerializeField] protected Transform firePoint;

    protected float cooldownTimer;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (CanShoot() && ShouldShoot())
        {
            Shoot();
            ResetCooldown();
        }
    }

    public virtual void SetWeapon(WeaponRuntimeItem runtime)
    {
        weaponRuntime = runtime;
    }

    protected virtual void ResetCooldown()
    {
        float fireRate = weaponRuntime?.baseData.fireRate ?? 5f;
        cooldownTimer = 1f / fireRate;
        Debug.Log($"[Shoot] Cooldown set = {cooldownTimer}s (fireRate = {fireRate} shots/sec)");
    }


    protected virtual bool CanShoot()
    {
        return weaponRuntime != null &&
               weaponRuntime.CanFire() &&
               cooldownTimer <= 0f;
    }

    protected abstract bool ShouldShoot();
    protected abstract void Shoot();
}
