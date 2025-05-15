using UnityEngine;

public class Bullet_Kinetic : BulletCtrl
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.GetComponentInParent<EntityStats>()?.gameObject ??
                            (collision.GetComponentInParent<IDamageable>() as Component)?.gameObject;

        if (target != null && ammoData != null)
        {
            DamageResolver.ApplyDamage(target, ammoData, owner);
        }

        Destroy(gameObject);
    }
}
