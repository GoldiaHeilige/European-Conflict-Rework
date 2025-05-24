using UnityEngine;

public class PenetrationResult
{
    public bool isPenetrated;
    public float finalDamage;
    public float durabilityLoss;
}

public static class PenetrationResolver
{
    public static PenetrationResult Resolve(AmmoData ammo, ArmorRuntime armor)
    {
        float duraRatio = armor.DurabilityRatio;
        int armorRating = armor.GetEffectiveRating();
        int pen = ammo.penetrationPower;
        int diff = pen - armorRating;

        float baseChance;

        if (diff <= -20)
            baseChance = 0f;
        else if (diff <= -10)
            baseChance = 0.1f;
        else if (diff < 0)
            baseChance = 0.2f;
        else if (diff == 0)
            baseChance = 0.3f;
        else if (diff <= 10)
            baseChance = 0.5f;
        else if (diff <= 20)
            baseChance = 0.8f;
        else
            baseChance = 0.95f;

        float bonusFromWeakArmor = (1f - duraRatio) * 0.25f;
        float finalChance = Mathf.Clamp01(baseChance + bonusFromWeakArmor);

        bool isPen = Random.value <= finalChance;

        float damage = isPen
            ? ammo.baseDamage
            : ammo.baseDamage * ammo.bluntDamageMultiplier;

        float duraLoss = pen * (isPen
            ? ammo.armorDamageMultiplierOnPen
            : ammo.armorDamageMultiplierOnFail);

        return new PenetrationResult
        {
            isPenetrated = isPen,
            finalDamage = damage,
            durabilityLoss = duraLoss
        };
    }
}
