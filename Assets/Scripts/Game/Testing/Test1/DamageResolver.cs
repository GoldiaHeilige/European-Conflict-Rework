
using UnityEngine;

public static class DamageResolver
{
    public static void ApplyDamage(GameObject target, AmmoData ammo, GameObject attacker)
    {
        var armorHandler = target.GetComponent<ArmorHandler>();

        float finalDamage = ammo.baseDamage;
        bool penetrated = true;

        ArmorSlot hitSlot = ArmorSlot.Body;
        ArmorRuntime hitArmor = null;

        if (armorHandler != null)
        {
            ArmorRuntime helmet = armorHandler.GetArmor(ArmorSlot.Head);
            ArmorRuntime body = armorHandler.GetArmor(ArmorSlot.Body);

            if (helmet != null && body != null)
            {
                float chance = Random.value;
                hitSlot = (chance <= 0.3f) ? ArmorSlot.Head : ArmorSlot.Body;
            }
            else if (helmet != null)
            {
                hitSlot = ArmorSlot.Head;
            }
            else if (body != null)
            {
                hitSlot = ArmorSlot.Body;
            }

            hitArmor = armorHandler.GetArmor(hitSlot);

            if (hitArmor != null)
            {
                int armorRating = hitArmor.armorData.armorRating;

                if (ammo.penetrationPower >= armorRating)
                {
                    penetrated = true;
                    finalDamage = ammo.baseDamage;
                    armorHandler.ReduceDurability(hitSlot, 5);
                }
                else
                {
                    penetrated = false;
                    finalDamage = ammo.baseDamage * 0.2f;
                    armorHandler.ReduceDurability(hitSlot, 1);
                }

                Debug.Log($"[DamageResolver] Bắn trúng {hitSlot} | Giáp = {armorRating} | PenPower = {ammo.penetrationPower} | Penetrated: {penetrated}");
            }
            else
            {
                Debug.Log($"[DamageResolver] Bắn trúng {hitSlot} nhưng không có giáp → full damage");
            }
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
