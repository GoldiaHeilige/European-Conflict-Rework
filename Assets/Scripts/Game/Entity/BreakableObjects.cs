using UnityEngine;

public enum BreakableResistType
{
    All,     
    ExplosiveOnly
}

public class Breakable : MonoBehaviour, IDamageable
{
    [Header("Destroy Settings")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private GameObject destroyedPrefab;  
    [SerializeField] private bool spawnEffect = true;
    [SerializeField] private BreakableResistType resistType = BreakableResistType.All;

    [Header("On Destroyed Effects")]
    [SerializeField] private GameObject onDestroyedEffect; 
    [SerializeField] private Vector2 effectOffset = Vector2.zero;

    private int currentHp;
    private bool isDestroyed = false;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDame(DameMessage msg)
    {
        if (isDestroyed) return;

        currentHp -= msg.Dame;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (spawnEffect && onDestroyedEffect)
        {
            Instantiate(onDestroyedEffect, transform.position + (Vector3)effectOffset, transform.rotation);
        }

        if (destroyedPrefab != null)
        {
            GameObject brokenObj = Instantiate(destroyedPrefab, transform.position, transform.rotation);
            brokenObj.transform.localScale = transform.localScale;
        }

        Destroy(gameObject);
    }

}
