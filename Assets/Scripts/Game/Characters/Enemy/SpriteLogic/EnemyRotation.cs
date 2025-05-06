using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [Header("References")]
    public Transform upperBody;
    public Transform target;

    void Update()
    {
        if (upperBody == null || target == null) return;

        Vector2 direction = (target.position - upperBody.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        upperBody.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }
}
