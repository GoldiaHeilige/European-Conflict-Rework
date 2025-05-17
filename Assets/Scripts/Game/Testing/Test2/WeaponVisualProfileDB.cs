using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Visuals/Weapon Visual DB")]
public class WeaponVisualProfileDB : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public WeaponData weapon;
        public Sprite sprite;
    }

    public Sprite unarmedSprite;
    public List<Entry> entries;

    public Sprite GetSpriteFor(WeaponRuntimeItem item)
    {
        if (item == null || item.baseData == null)
            return unarmedSprite;

        foreach (var e in entries)
        {
            if (e.weapon == item.baseData)
                return e.sprite;
        }

        return unarmedSprite;
    }
}