using UnityEngine;

[System.Serializable]
public class LootDropEntry
{
    public InventoryItemData itemData;

    [Header("Drop Chance Range (%)")]
    [Range(0f, 1f)] public float chanceMin = 0.1f;
    [Range(0f, 1f)] public float chanceMax = 0.2f;

    [Header("Drop Amount Range")]
    public int amountMin = 1;
    public int amountMax = 3;

    public float GetRandomChance()
    {
        return Random.Range(chanceMin, chanceMax);
    }

    public int GetRandomAmount()
    {
        return Random.Range(amountMin, amountMax + 1);
    }
}
