using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float radius = 2f;
    public int damage = 30;

    public void ApplyDamage(Vector2 position, GameObject source)
    {
        Debug.Log("Explosion: bắt đầu gây sát thương!");

        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radius);
        foreach (var col in targets)
        {
            if (col.gameObject == gameObject) continue;

            IDamageable damageable = col.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDame(new DameMessage
                {
                    Dame = damage,
                    Attacker = source
                });
            }
            else
            {
                Debug.Log($"Không tìm thấy IDamageable trên {col.gameObject.name}");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
