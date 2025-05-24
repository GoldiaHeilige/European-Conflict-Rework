using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public List<LootDropEntry> lootList;

    public void DropLoot()
    {
        foreach (var entry in lootList)
        {
            if (entry.itemData == null) continue;

            float roll = Random.value;
            float randomChance = entry.GetRandomChance();

            if (roll <= randomChance)
            {
                int amount = entry.GetRandomAmount();

                var runtime = new InventoryItemRuntime(
                    entry.itemData,
                    amount,
                    null,
                    System.Guid.NewGuid().ToString()
                );

                DropSpawner.Instance.Spawn(runtime, true);
                Debug.Log($"[LootDrop] Rơi {entry.itemData.name} | amount = {amount} | roll = {roll:F2} <= {randomChance:F2}");
            }
            else
            {
                Debug.Log($"[LootDrop] KHÔNG rơi {entry.itemData.name} | roll = {roll:F2} > {randomChance:F2}");
            }
        }
    }
}
