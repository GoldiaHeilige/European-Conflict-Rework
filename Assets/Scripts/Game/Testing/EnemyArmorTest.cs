using UnityEngine;

public class EnemyArmorTest : MonoBehaviour
{
    [Header("Chọn giáp cho enemy")]
    public ArmorData helmetData;
    public ArmorData bodyArmorData;

    private EquippedArmorManager armorManager;

    private void Start()
    {
        armorManager = GetComponent<EquippedArmorManager>();
        if (armorManager == null)
        {
            Debug.LogError("Thiếu EquippedArmorManager trên enemy!");
            return;
        }

        EquipArmor(helmetData);
        EquipArmor(bodyArmorData);
    }

    private void EquipArmor(ArmorData data)
    {
        if (data == null) return;

        var runtime = new ArmorRuntime(data, armorManager, new InventoryItemRuntime(data, 1, data.maxDurability));

        armorManager.EquipArmor(runtime, null);

        Debug.Log($"Enemy trang bị: {data.itemName}");
    }
}
