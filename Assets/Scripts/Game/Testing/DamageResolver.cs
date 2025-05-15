using UnityEngine;

public static class DamageResolver
{
    public static void ApplyDamage(GameObject target, AmmoData ammo, GameObject attacker)
    {
        var armorHandler = target.GetComponent<ArmorHandler>();
        var stats = target.GetComponent<EntityStats>();

        bool penetrated = false;
        float finalDamage = ammo.baseDamage;

        if (armorHandler != null)
        {
            int armorRating = armorHandler.CalculateTotalArmorRating();
            penetrated = ammo.penetrationPower >= armorRating;
            finalDamage = penetrated ? ammo.baseDamage : ammo.baseDamage * 0.2f;
            armorHandler.ApplyDurability(penetrated);
        }

        if (stats != null)
        {
            stats.ApplyDamage(finalDamage);
        }

        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDame(new DameMessage
            {
                Damage = Mathf.RoundToInt(finalDamage),
                Attacker = attacker,
                AmmoUsed = ammo
            });
        }
    }
}
