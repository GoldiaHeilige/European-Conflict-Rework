using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float radius;

    public void SetDamage(int value)
    {
        damage = value;
        // Debug.Log($"[ExplosionDamage] Damage set to: {damage}");
    }

    public void ApplyDamage(Vector2 center, GameObject source)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        // Debug.Log($"[ExplosionDamage] Found {hits.Length} objects in radius {radius}");

        int total = 0;

        foreach (var hit in hits)
        {
            if (hit == null || hit.gameObject == null) continue;

            if (hit.gameObject == gameObject)
                continue;

            if (hit.CompareTag("PlayerBullet") || hit.CompareTag("EnemyBullet"))
                continue;

            var target = hit.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                target.TakeDame(new DameMessage
                {
                    Dame = damage,
                    Attacker = source
                });

                // Debug.Log($"Explosion gây {damage} dame lên {hit.name}");
                total++;
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy IDamageable trên {hit.name}");
            }
        }

        // Debug.Log($"Explosion từ {(source ? source.name : "?")} gây damage cho {total} mục tiêu");
    }

}
