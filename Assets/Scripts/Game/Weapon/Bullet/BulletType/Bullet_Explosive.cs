using UnityEngine;

public class Bullet_Explosive : BulletCtrl
{
    [SerializeField] private GameObject explosionPrefab;

    public override void SetOwnerAndDamage(GameObject owner, int damage)
    {
        base.SetOwnerAndDamage(owner, damage);
        // Debug.Log($"[Bullet_Explosive] Set owner = {owner?.name ?? "?"}, damage = {damage}");
    }


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

            if (explosionDamage != null)
            {
                explosionDamage.SetDamage(damage);
                explosionDamage.SetBulletType(bulletType);
                explosionDamage.ApplyDamage(transform.position, owner);
            }
            else
            {
                Debug.LogWarning("❌ Prefab animation nổ không có ExplosionDamage");
            }
        }
        else
        {
            Debug.LogWarning(" explosionPrefab là NULL trong Bullet_Explosive.");
        }
    }
}
