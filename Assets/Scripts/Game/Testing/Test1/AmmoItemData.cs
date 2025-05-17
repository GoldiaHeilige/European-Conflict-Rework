using UnityEngine;

[CreateAssetMenu(fileName = "NewAmmoItem", menuName = "Items/Ammo Item")]
public class AmmoItemData : InventoryItemData
{
    [Header("Link AmmoData with AmmoItem")]
    public AmmoData linkedAmmoData;
}
