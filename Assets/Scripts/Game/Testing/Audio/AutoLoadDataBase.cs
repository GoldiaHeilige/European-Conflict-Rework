using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AutoLoad Sound Database")]
public class AutoLoadSoundDatabase : ScriptableObject
{
    [Header("Sound List (loaded automatically)")]
    public List<SoundDataFlexible> allSounds;

#if UNITY_EDITOR
    [ContextMenu("Auto Reload All From Resources/Audio")]
    public void AutoReload()
    {
        allSounds = new List<SoundDataFlexible>(Resources.LoadAll<SoundDataFlexible>("Audio/"));
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"[SoundDatabase] Đã load {allSounds.Count} sound từ Resources/Audio");
    }
#endif
}
