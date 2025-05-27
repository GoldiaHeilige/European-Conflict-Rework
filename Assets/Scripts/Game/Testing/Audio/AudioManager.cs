using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Dictionary<string, List<SoundDataFlexible>> soundMap;
    private List<AudioHandle> activeSounds = new List<AudioHandle>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AutoLoadFromResources(); // ⬅️ tự động load
            BuildSoundMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void AutoLoadFromResources()
    {
        var loadedSounds = Resources.LoadAll<SoundDataFlexible>("Audio/");
        Debug.Log($"[AudioManager] Loaded {loadedSounds.Length} SoundDataFlexible from Resources/Audio/");
        soundMap = new Dictionary<string, List<SoundDataFlexible>>();

        foreach (var data in loadedSounds)
        {
            if (data == null) continue;

            string key = data.GetKey();
            if (!soundMap.ContainsKey(key))
                soundMap[key] = new List<SoundDataFlexible>();

            soundMap[key].Add(data);
        }
    }

    void BuildSoundMap()
    {
        // giữ lại để tương thích nếu bạn vẫn muốn gọi BuildSoundMap()
    }

    public void Play(string category, string subKey)
    {
        string key = $"{category}_{subKey}";
        if (soundMap.ContainsKey(key))
        {
            var list = soundMap[key];
            var sound = list[Random.Range(0, list.Count)];
            var clip = sound.GetRandomClip();
            if (clip != null)
                sfxSource.PlayOneShot(clip, sound.volume);
        }
    }

    public void PlayMusic(string category, string subKey, bool loop = true)
    {
        string key = $"{category}_{subKey}";
        if (soundMap.ContainsKey(key))
        {
            var list = soundMap[key];
            var sound = list[Random.Range(0, list.Count)];
            var clip = sound.GetRandomClip();
            if (clip != null)
            {
                musicSource.clip = clip;
                musicSource.volume = sound.volume;
                musicSource.loop = loop;
                musicSource.Play();
            }
        }
    }

    public AudioHandle PlayLooping(string category, string subKey, string tag = null)
    {
        string key = $"{category}_{subKey}";
        if (soundMap.ContainsKey(key))
        {
            var data = soundMap[key][Random.Range(0, soundMap[key].Count)];
            var clip = data.GetRandomClip();
            if (clip != null)
            {
                GameObject temp = new GameObject("AudioRuntime_" + tag);
                temp.transform.parent = this.transform;

                AudioSource src = temp.AddComponent<AudioSource>();
                src.clip = clip;
                src.volume = data.volume;
                src.loop = false;
                src.Play();

                Destroy(temp, clip.length + 0.2f);

                var handle = new AudioHandle { source = src, tag = tag };
                activeSounds.Add(handle);
                Debug.Log($"[PlayLooping] Tag: {tag} | Clip: {clip?.name}");
                return handle;
            }
        }
        return null;
    }

    public void StopByTag(string tag)
    {
        for (int i = activeSounds.Count - 1; i >= 0; i--)
        {
            if (activeSounds[i].tag == tag)
            {
                activeSounds[i].Stop();
                Destroy(activeSounds[i].source.gameObject);
                activeSounds.RemoveAt(i);
            }
        }
    }

    public void StopIfPlaying(string category, string subKey)
    {
        string key = $"{category}_{subKey}";
        if (musicSource != null && musicSource.isPlaying)
        {
            Debug.Log($"[StopIfPlaying] Đang kiểm tra key: {key}, hiện đang phát clip: {musicSource.clip.name}");

            if (soundMap.ContainsKey(key))
            {
                foreach (var sound in soundMap[key])
                {
                    if (sound.clips.Contains(musicSource.clip))
                    {
                        Debug.Log($"[StopIfPlaying] Dừng nhạc: {musicSource.clip.name}");
                        musicSource.Stop();
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[StopIfPlaying] Không tìm thấy key: {key} trong soundMap.");
            }
        }
        else
        {
            Debug.LogWarning("[StopIfPlaying] musicSource chưa phát hoặc null");
        }
    }

}
