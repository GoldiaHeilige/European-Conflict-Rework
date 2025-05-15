using UnityEngine;

public class Bullet_Explosive : BulletCtrl
{
    [SerializeField] private GameObject explosionPrefab;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            var explosionDamage = explosion.GetComponent<ExplosionDamage>();

            if (explosionDamage != null && ammoData != null)
            {
                explosionDamage.ApplyDamage(transform.position, owner, ammoData);
            }
            else
            {
                Debug.LogWarning("❌ Prefab nổ thiếu hoặc ammoData NULL");
            }
        }
    }
}
