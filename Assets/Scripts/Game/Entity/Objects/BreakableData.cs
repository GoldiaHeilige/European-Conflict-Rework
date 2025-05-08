using UnityEngine;

[CreateAssetMenu(menuName = "EC/Breakable Data")]
public class BreakableData : ScriptableObject
{
    [Tooltip("Object Settings")]
    public BulletType[] validBulletTypes;
    public int maxHp = 100;

    [Header("On destroyed Sprite & Effects")]
    public GameObject destroyedPrefab;
    public GameObject destroyEffect;
}
