using UnityEngine;
using System.Collections;

public class MusicStopper : MonoBehaviour
{
    [Header("Category/SubKey để dừng")]
    public string category = "Music";
    public string subKey = "";

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopIfPlaying(category, subKey);
        }
        Destroy(gameObject);
    }
}
