using UnityEngine;
using TMPro;

public class PickupDetector : MonoBehaviour
{
    public LayerMask pickupLayer;
    public Camera mainCam;
    public float maxDistance = 5f;

    [Header("UI")]
    public GameObject tooltipUI;
    public TextMeshProUGUI tooltipText;

    private PickupItem currentItem;

    private void Update()
    {
        DetectItemUnderMouse();
        HandlePickupInput();
    }

    void DetectItemUnderMouse()
    {
        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero, Mathf.Infinity, pickupLayer);

        if (hit.collider != null && hit.collider.TryGetComponent(out PickupItem item))
        {
            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist > maxDistance)
            {
                tooltipUI.SetActive(false);
                currentItem = null;
                return;
            }

            item.isHovered = true;
            currentItem = item;

            tooltipUI.SetActive(true);
            tooltipText.text = $"<sprite=0> Press [F] to pickup {item.itemData.itemName}";
            tooltipUI.transform.position = Input.mousePosition;
        }
        else
        {
            currentItem = null;
            tooltipUI.SetActive(false);
        }
    }


    void HandlePickupInput()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.F))
        {
            currentItem.Pickup();
            currentItem = null;
            tooltipUI.SetActive(false);
        }
    }
}
