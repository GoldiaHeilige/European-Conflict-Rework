using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Player
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10f);

    public float minZoom = 4f;
    public float maxZoom = 8f;
    public float zoomSpeed = 10f;

    void LateUpdate()
    {
        // Camera follow
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Camera zoom in or out based on the distance bettween the mouse cursor (or whatever is it on mobile (not yet finished) and the player)
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        float distance = Vector2.Distance(target.position, mouseWorldPos);

        float targetZoom = Mathf.Clamp(minZoom + distance / 4f, minZoom, maxZoom);

        float currentZoom = Camera.main.orthographicSize;

        float newZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);

        Camera.main.orthographicSize = newZoom;
    }
}
