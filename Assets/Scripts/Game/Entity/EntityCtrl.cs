using UnityEngine;

public class EntityCtrl : MonoBehaviour
{
    protected EntityStats stats;

    protected virtual void Awake()
    {
        stats = GetComponent<EntityStats>();
        if (stats == null)
        {
            Debug.LogError("thiếu EntityStats trên " + gameObject.name);
        }
        else
        {
            stats.OnDie += Die;
        }
    }

    public virtual void TakeDamage(int amount, GameObject source = null)
    {
        stats?.TakeDamage(amount, source);
    }

    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} đã chết (base Die)");
    }

    public float MoveSpeed => stats?.MoveSpeed ?? 0f;
}
