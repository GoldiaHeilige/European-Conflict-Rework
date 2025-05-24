using UnityEngine;
using System;

public class EntityStats : MonoBehaviour, IDamageable
{
    public int health;
    public float moveSpeed;

    private int currentHP;
    private EquippedArmorManager armorManager;

    public int CurrentHP => currentHP;
    public int MaxHP => health;
    public float MoveSpeed => moveSpeed;

    public bool IsFullHealth => currentHP >= health;
    public bool IsAlive => currentHP > 0;

    public event Action OnDie;
    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        currentHP = health;
        armorManager = GetComponent<EquippedArmorManager>();
        OnHealthChanged?.Invoke(currentHP);
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

    public void Heal(int amount)
    {
        int before = currentHP;
        currentHP = Mathf.Min(currentHP + amount, health);
        OnHealthChanged?.Invoke(currentHP);

        Debug.Log($"{gameObject.name} được hồi {amount} máu → {before} → {currentHP}");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} đã chết.");
        OnDie?.Invoke();
    }
}
