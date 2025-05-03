using UnityEngine;

public class EnemyStatsCtrl : MonoBehaviour
{
    private EntityStats stats;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        if (stats != null)
        {
            stats.OnDie += HandleDeath;
        }
    }

    private void HandleDeath()
    {
        Debug.Log("Enemy đã chết");
        Destroy(gameObject);
    }
}
