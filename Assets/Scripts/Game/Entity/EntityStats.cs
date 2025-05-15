using UnityEngine;
using System;

public class EntityStats : MonoBehaviour, IDamageable
{
    public int health;
    public float moveSpeed;

    private int currentHP;
    private EquippedArmorManager armorManager;

    public int CurrentHP => currentHP;
    public float MoveSpeed => moveSpeed;

    public event Action OnDie;
    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        currentHP = health;
        armorManager = GetComponent<EquippedArmorManager>();
    }

    public void ApplyDamage(float amount)
    {
        health -= Mathf.RoundToInt(amount);
        OnHealthChanged?.Invoke(health);

        Debug.Log($"{gameObject.name} mất {amount} máu (còn lại: {health})");

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDame(DameMessage message)
    {
        ApplyDamage(message.Damage);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} đã chết.");
        OnDie?.Invoke(); // gọi sự kiện
        Destroy(gameObject);
    }
}
