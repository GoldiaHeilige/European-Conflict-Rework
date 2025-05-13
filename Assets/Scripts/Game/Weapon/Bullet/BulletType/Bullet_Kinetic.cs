using UnityEngine;

public class Bullet_Kinetic : BulletCtrl
{
    /*    protected override void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                var message = new DameMessage
                {
                    Dame = damage,
                    Attacker = owner
                };
                target.TakeDame(message);

                // Debug.Log($"{owner?.name ?? "?"} gây {damage} damage → {collision.name}");
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy IDamageable trên {collision.name}");
            }

            Destroy(gameObject);
        }*/

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var stats = collision.GetComponentInParent<EntityStats>();
        if (stats != null)
        {
            ArmorUtils.ApplyDamageTo(stats, damage, penetrationLevel);
        }
        else
        {
            var target = collision.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                var message = new DameMessage
                {
                    Dame = damage,
                    Attacker = owner,
                    BulletType = BulletType.Kinetic
                };
                target.TakeDame(message);
            }
        }

        Destroy(gameObject);
    }

}
