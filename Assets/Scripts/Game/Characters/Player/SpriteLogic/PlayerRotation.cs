using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    public static bool allowMouseLook = true;

    void Update()
    {
        if (!allowMouseLook) return;
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
