using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float radius = 3f;

    public void ApplyDamage(Vector2 center, GameObject source, AmmoData ammo)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);

        foreach (var hit in hits)
        {
            if (hit == null || hit.gameObject == null) continue;
            if (hit.gameObject == gameObject) continue;
            if (hit.CompareTag("PlayerBullet") || hit.CompareTag("EnemyBullet")) continue;

            GameObject target = hit.GetComponentInParent<EntityStats>()?.gameObject ??
                                (hit.GetComponentInParent<IDamageable>() as Component)?.gameObject;

            if (target != null && ammo != null)
            {
                DamageResolver.ApplyDamage(target, ammo, source);
            }
        }
    }
}