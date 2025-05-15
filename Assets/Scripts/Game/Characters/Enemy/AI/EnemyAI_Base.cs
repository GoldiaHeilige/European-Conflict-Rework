using UnityEngine;

public abstract class EnemyAI_Base : MonoBehaviour
{
    protected EnemyController controller;

    protected virtual void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    protected abstract void Update();
}
