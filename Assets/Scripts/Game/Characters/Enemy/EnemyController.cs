using UnityEngine;

public class EnemyController : EntityCtrl
{
    [Header("Enemy Settings")]
    public string enemyType;

    private bool isDead = false;

    private void Start()
    {
        // Debug.Log($"Spawn Enemy: {gameObject.name} loại {enemyType}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            if (other.TryGetComponent<BulletCtrl>(out var baseBullet))
            {
                if (!(baseBullet is Bullet_Explosive))
                {
                    var msg = new DameMessage
                    {
                        Dame = baseBullet.GetDamage(),
                        Attacker = baseBullet.GetOwner()
                    };

                    TakeDame(msg);
                    Destroy(other.gameObject);
                }
            }
        }
    }


    public void TakeDame(DameMessage msg)
    {

        TakeDamage(msg.Dame, msg.Attacker);
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;

        base.Die();
    }
}
