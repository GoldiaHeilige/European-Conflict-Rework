[System.Serializable]
public class InventoryItemRuntime
{
    public InventoryItemData itemData;
    public int quantity;
    public int durability; 

    public InventoryItemRuntime(InventoryItemData data, int qty)
    {
        itemData = data;
        quantity = qty;

        if (data is ArmorData armor)
        {
            durability = armor.maxDurability;
        }
    }
}
