using UnityEngine;

public class EntityCtrl : MonoBehaviour, IDamageable
{
    [Header("Entity Stats")]
    public float maxHealth;
    [HideInInspector] public float currentHealth;

    public float baseMoveSpeed;
    [HideInInspector] public float moveSpeed;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        moveSpeed = baseMoveSpeed;
    }

    public virtual void TakeDame(DameMessage message)
    {
        currentHealth -= message.Dame;
        Debug.Log($"📉 {gameObject.name} bị {message.Dame} damage từ {message.Attacker?.name}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject, 1.5f); 
    }
}
