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

    protected new virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;

        /*Debug.Log($"🟢 Bullet_Kinetic: Va chạm với {collision.name}, tag = {collision.tag}");*/

        bool isHit = (owner.CompareTag("Player") && collision.CompareTag("Enemy")) ||
                     (owner.CompareTag("Enemy") && collision.CompareTag("Player"));

        if (isHit)
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
                Debug.Log($"Gây {damage} damage từ {owner.name} → {collision.name}");
            }
            else
            {
                Debug.LogWarning("không tìm thấy IDamageable trên target");
            }

            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
