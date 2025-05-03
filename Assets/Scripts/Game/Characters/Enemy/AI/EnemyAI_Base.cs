using UnityEngine;

public abstract class EnemyAI_Base : MonoBehaviour
{
    protected EnemyShooting shooting;
    protected EnemyReload reloading;
    protected Transform player;

    protected virtual void Start()
    {
        shooting = GetComponent<EnemyShooting>();
        reloading = GetComponent<EnemyReload>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (player == null || shooting == null || reloading == null) return;

        HandleAI();
    }

    protected abstract void HandleAI();
}
