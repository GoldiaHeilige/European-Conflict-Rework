using UnityEngine;

public class Bullet_Kinetic : BulletCtrl
{
    private GameObject owner;
    private int damage;

    public void SetOwnerAndDamage(GameObject owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;
/*        Debug.Log($"Bullet_Kinetic: Gán owner = {owner?.name}, damage = {damage}");*/
    }

    public int GetDamage() => damage;
    public GameObject GetOwner() => owner;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponentInParent<IDamageable>();

        if (target != null)
        {
            if (owner == null)
            {
                Debug.LogWarning("❌ Bullet owner NULL – có thể chưa gán đúng!");
            }

            var message = new DameMessage
            {
                Dame = damage,
                Attacker = owner
            };

            target.TakeDame(message);
            Debug.Log($"Gây {damage} damage từ {owner?.name ?? "NULL"} → {collision.name}");
        }
        else
        {
            Debug.LogWarning($"❌ Không tìm thấy IDamageable trên {collision.name}");
        }

        Destroy(gameObject);
    }
}
