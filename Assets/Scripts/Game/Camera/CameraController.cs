using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform target; 
    public CinemachineVirtualCamera virtualCam;

    public float minZoom = 4f;
    public float maxZoom = 8f;
    public float zoomSpeed = 10f;

    public float maxDisplacement = 4f;
    public float offsetLerpSpeed = 5f; 

    private CinemachineFramingTransposer framingTransposer;
    private Vector3 currentOffset = Vector3.zero;

    private void Start()
    {
        if (virtualCam != null && target != null)
        {
            virtualCam.Follow = target;

            framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    void Update()
    {
        if (virtualCam == null || target == null || framingTransposer == null) return;

        // Displacement logic
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 dir = ((Vector2)mouseWorldPos - (Vector2)target.position).normalized;
        float distance = Vector2.Distance(target.position, mouseWorldPos);

        // Scale offset based on mouse distance, not going pass maxDisplacement
        Vector3 targetOffset = (Vector3)(dir * Mathf.Min(distance, maxDisplacement));

        // Lerp for smoother offset
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * offsetLerpSpeed);

        framingTransposer.m_TrackedObjectOffset = new Vector3(currentOffset.x, currentOffset.y, 0);

        // Zooming logic
        float targetZoom = Mathf.Clamp(minZoom + distance / 4f, minZoom, maxZoom);
        float currentZoom = virtualCam.m_Lens.OrthographicSize;
        float newZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        virtualCam.m_Lens.OrthographicSize = newZoom;
    }
}
