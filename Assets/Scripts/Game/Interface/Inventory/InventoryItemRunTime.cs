[System.Serializable]
public class InventoryItemRuntime
{
    public InventoryItemData itemData;
    public int quantity;

    public InventoryItemRuntime(InventoryItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
    }
}
