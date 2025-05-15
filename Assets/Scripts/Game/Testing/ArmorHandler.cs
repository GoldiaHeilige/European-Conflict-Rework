using UnityEngine;

public class ArmorHandler : MonoBehaviour
{
    private EquippedArmorManager armorManager;

    private void Awake()
    {
        armorManager = GetComponent<EquippedArmorManager>();
        if (armorManager == null)
        {
            Debug.LogError("Không tìm thấy EquippedArmorManager trên Player!");
        }
    }

    public int CalculateTotalArmorRating()
    {
        ArmorRuntime helmet = armorManager?.GetArmor(ArmorSlot.Head);
        ArmorRuntime body = armorManager?.GetArmor(ArmorSlot.Body);

        int helmetAR = helmet?.GetProtection() ?? 0;
        int bodyAR = body?.GetProtection() ?? 0;

        return Mathf.RoundToInt(helmetAR * 0.3f + bodyAR * 0.7f);
    }

    public void ApplyDurability(bool penetrated)
    {
        ArmorRuntime helmet = armorManager?.GetArmor(ArmorSlot.Head);
        ArmorRuntime body = armorManager?.GetArmor(ArmorSlot.Body);

        if (helmet != null)
        {
            int damage = penetrated ? 5 : 1;
            helmet.durability = Mathf.Max(helmet.durability - damage, 0);
        }

        if (body != null)
        {
            int damage = penetrated ? 10 : 2;
            body.durability = Mathf.Max(body.durability - damage, 0);
        }
    }
}
