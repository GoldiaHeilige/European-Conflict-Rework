using UnityEngine;
using TMPro;

public class ArmorStatusUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    private EquippedArmorManager armorMgr;

    private void Start()
    {
        armorMgr = FindObjectOfType<EquippedArmorManager>();
        UpdateDisplay();

        PlayerInventory.InventoryChanged += UpdateDisplay;
    }

    private void OnDestroy()
    {
        PlayerInventory.InventoryChanged -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        if (armorMgr == null || statusText == null)
            return;

        int count = 0;

        foreach (var armor in armorMgr.GetAllEquippedArmors())
        {
            if (armor != null && armor.armorData != null && armor.currentDurability > 0)
            {
                count++;
            }
        }

        switch (count)
        {
            case 0:
                statusText.text = "NONE";
                statusText.color = Color.red;
                break;
            case 1:
                statusText.text = "HALF";
                statusText.color = Color.yellow;
                break;
            case 2:
                statusText.text = "OK";
                statusText.color = Color.green;
                break;
            default:
                statusText.text = "??";
                statusText.color = Color.white;
                break;
        }
    }
}
