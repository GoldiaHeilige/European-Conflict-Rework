using UnityEngine;
using TMPro;

public class ArmorDebugDisplay : MonoBehaviour
{
    [Header("Tham chiếu đến UI Text")]
    public TextMeshProUGUI helmetText;
    public TextMeshProUGUI bodyText;

    [Header("Nguồn dữ liệu")]
    public EquippedArmorManager armorManager;

    private void Update()
    {
        if (armorManager == null) return;

        var helmet = armorManager.GetArmor(ArmorSlot.Head);
        var body = armorManager.GetArmor(ArmorSlot.Body);

        if (helmet != null)
        {
            helmetText.text = $"Helmet\nAR: {helmet.GetProtection()}\nHP: {helmet.durability}/{helmet.armorData.maxDurability}";
        }
        else
        {
            helmetText.text = "Helmet: NONE";
        }

        if (body != null)
        {
            bodyText.text = $"Body\nAR: {body.GetProtection()}\nHP: {body.durability}/{body.armorData.maxDurability}";
        }
        else
        {
            bodyText.text = "Body: NONE";
        }

    }
}
