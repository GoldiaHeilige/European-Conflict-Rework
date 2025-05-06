using UnityEngine;

public class EnemyController : EntityCtrl
{
    [Header("Enemy Settings")]
    public string enemyType;

    private void Start()
    {
        // Debug.Log($"Spawn Enemy: {gameObject.name} loại {enemyType}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            BulletCtrl baseBullet = other.GetComponent<BulletCtrl>();
            // Debug.Log($"🟢 EnemyCtrl: Va chạm với đạn {other.gameObject.name}");

            if (baseBullet is Bullet_Kinetic kinetic)
            {
                // Debug.Log("Bullet là Bullet_Kinetic, xử lý gây damage");

                var msg = new DameMessage
                {
                    Dame = kinetic.GetDamage(),
                    Attacker = kinetic.GetOwner()
                };

                TakeDame(msg);
            }
            else
            {
                Debug.LogWarning("Không phải Bullet_Kinetic, bỏ qua xử lý damage.");
            }

            Destroy(other.gameObject);
        }
    }

    public void TakeDame(DameMessage msg)
    {
        TakeDamage(msg.Dame, msg.Attacker);
    }

    public override void Die()
    {
        base.Die();
    }
}
