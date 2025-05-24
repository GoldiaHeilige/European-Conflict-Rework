using UnityEngine;

[CreateAssetMenu(menuName = "EC/Breakable Data")]
public class BreakableData : ScriptableObject
{
    [Tooltip("Can get destroyed by ->")]
    public AmmoData[] validAmmoTypes;

    public int maxHp = 50;

    [Header("On Destroyed Effects")]
    public GameObject destroyedPrefab;
    public GameObject destroyEffect;
}

