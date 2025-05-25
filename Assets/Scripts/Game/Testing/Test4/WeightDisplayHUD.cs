using UnityEngine;
using TMPro;

public class WeightDisplayHUD : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI weightText;

    private void Awake()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        UpdateWeightDisplay();
    }

    private void OnEnable()
    {
        PlayerInventory.InventoryChanged += UpdateWeightDisplay;
    }

    private void OnDisable()
    {
        PlayerInventory.InventoryChanged -= UpdateWeightDisplay;
    }

    private void UpdateWeightDisplay()
    {
        if (playerInventory == null || weightText == null) return;

        float weight = playerInventory.TotalWeight;
        float soft = playerInventory.SoftLimit;
        float max = playerInventory.MaxWeight;

        weightText.text = $"{weight:0.0} kg";

        if (weight > max)
            weightText.color = Color.red;
        else if (weight > soft)
            weightText.color = Color.yellow;
        else
            weightText.color = Color.white;

        Debug.Log($"⚖ [HUD] Tổng trọng lượng: {weight}");
    }

    public void ForceUpdate()
    {
        UpdateWeightDisplay();
    }
}
