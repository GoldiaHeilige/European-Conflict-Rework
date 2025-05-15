using UnityEngine;

public class EnemyStatsCtrl : MonoBehaviour
{
    private EntityStats stats;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        if (stats != null)
        {
            stats.OnDie += OnDeath;
        }
    }

    private void OnDeath()
    {
        Debug.Log("Enemy chết.");
        Destroy(gameObject);
    }
}
