using UnityEngine;
using System;

public class EntityStats : MonoBehaviour, IDamageable
{
    public int maxHP;
    public float moveSpeed;

    private int currentHP;

    public int CurrentHP => currentHP;
    public float MoveSpeed => moveSpeed;

    public event Action OnDie;
    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount, GameObject source = null)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);

        OnHealthChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void TakeDame(DameMessage msg)
    {
        TakeDamage(msg.Dame, msg.Attacker);
    }
}
