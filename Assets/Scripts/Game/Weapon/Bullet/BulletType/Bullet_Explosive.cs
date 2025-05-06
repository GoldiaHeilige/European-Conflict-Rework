using UnityEngine;

public class Bullet_Explosive : BulletCtrl
{
    [Header("Effect")]
    public GameObject explosionEffectPrefab;

    private GameObject owner;
    private int damage;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // 1. Explosion Effects
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. AOE damage
        ExplosionDamage explosion = GetComponent<ExplosionDamage>();
        if (explosion != null)
        {
            explosion.ApplyDamage(transform.position, gameObject);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ExplosionDamage trên Bullet_Explosive");
        }
    }

    public void SetOwnerAndDamage(GameObject owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

/*    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }*/
}
