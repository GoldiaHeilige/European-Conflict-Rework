using UnityEngine;

public class AudioHandle
{
    public AudioSource source;
    public string tag; // để phân loại như "Reload", "Footstep"

    public void Stop()
    {
        if (source && source.isPlaying)
        {
            source.Stop();
        }
    }
}
