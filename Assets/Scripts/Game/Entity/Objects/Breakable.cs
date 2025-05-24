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

        if (!IsAmmoValid(msg.AmmoUsed))
        {
            Debug.Log($"{gameObject.name} không bị phá bởi loại đạn {msg.AmmoUsed?.name ?? "NULL"}");
            return;
        }

        currentHp -= msg.Damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private bool IsAmmoValid(AmmoData ammo)
    {
        return ammo != null && data.validAmmoTypes != null && data.validAmmoTypes.Contains(ammo);
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

        var dropper = GetComponent<LootDropper>();
        if (dropper != null)
        {
            dropper.DropLoot();
        }


        Destroy(gameObject);
    }
}
