using UnityEngine;
using System.Linq;

public class Breakable : MonoBehaviour, IDamageable
{
    [SerializeField] private BreakableData data;

    private int currentHp;
    private bool isDestroyed = false;

    private void Awake()
    {
        currentHp = data.maxHp;
    }

    public void TakeDame(DameMessage msg)
    {
        if (isDestroyed) return;

        if (!IsBulletValid(msg.BulletType))
        {
            Debug.Log($"{gameObject.name} không bị phá bởi đạn loại {msg.BulletType}");
            return;
        }

        currentHp -= msg.Dame;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private bool IsBulletValid(BulletType bulletType)
    {
        return data.validBulletTypes != null && data.validBulletTypes.Contains(bulletType);
    }

    private void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (data.destroyEffect)
            Instantiate(data.destroyEffect, transform.position, Quaternion.identity);

        if (data.destroyedPrefab)
        {
            var obj = Instantiate(data.destroyedPrefab, transform.position, transform.rotation);
            obj.transform.localScale = transform.localScale;
        }

        Destroy(gameObject);
    }
}
