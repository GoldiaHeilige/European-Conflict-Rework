using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio/FlexibleSoundData")]
public class SoundDataFlexible : ScriptableObject
{
    public SoundKey key;
    public List<AudioClip> clips;
    public float volume = 1f;
    [Range(0f, 1f)] public float playChance = 1f;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Count == 0 || Random.value > playChance)
            return null;

        return clips[Random.Range(0, clips.Count)];
    }

    public string GetKey() => $"{key.category}_{key.subKey}";
}
