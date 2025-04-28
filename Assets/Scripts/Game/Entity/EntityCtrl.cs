using UnityEngine;

public class EntityCtrl : MonoBehaviour
{
    [Header("Entity Stats")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    public float baseMoveSpeed = 5f;
    [HideInInspector] public float moveSpeed;

    [Header("Components")]
    public Animator animator;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        moveSpeed = baseMoveSpeed;
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f); 
    }
}
