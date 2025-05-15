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
        Debug.Log("Player đã chết!");

        var controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Có thể thêm hiệu ứng hoặc chuyển scene ở đây
    }
}
