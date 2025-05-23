
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

    public void TakeDame(DameMessage message)
    {
        ApplyDamageInternal(message.Damage);
    }

    private void ApplyDamageInternal(float amount)
    {
        currentHP -= Mathf.RoundToInt(amount);
        OnHealthChanged?.Invoke(currentHP);

        Debug.Log($"{gameObject.name} mất {amount} máu (còn lại: {currentHP})");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} đã chết.");
        OnDie?.Invoke(); // gọi sự kiện
        Destroy(gameObject);
    }
}
