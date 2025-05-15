using UnityEngine;

[CreateAssetMenu(menuName = "EC/Breakable Data")]
public class BreakableData : ScriptableObject
{
    [Tooltip("Object Settings")]
    public AmmoData[] validAmmoTypes;

    public int maxHp = 50;

    [Header("On Destroyed Effects")]
    public GameObject destroyedPrefab;
    public GameObject destroyEffect;
}

