using UnityEngine;

public class PlayerStatsCtrl : MonoBehaviour
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

    }
}
