using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;
    [HideInInspector] public bool isHovered = false;

    private SpriteRenderer sr;
    private Color normalColor;
    public Color glowColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
    }

    private void Update()
    {
        if (isHovered) sr.color = glowColor;
        else sr.color = normalColor;

        isHovered = false;
    }

    public void Pickup()
    {
        PlayerInventory.Instance.AddItem(itemData, amount);
        Destroy(gameObject);
    }
}
