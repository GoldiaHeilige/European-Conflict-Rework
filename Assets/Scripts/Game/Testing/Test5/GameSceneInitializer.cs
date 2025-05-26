using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Start()
    {
        // Bật chuột xoay camera lại
        PlayerRotation.allowMouseLook = true;

        // Reset time scale
        Time.timeScale = 1f;

        // Nếu bạn dùng stack UI → clear khi vào scene mới
/*        UIStackClose.Clear();*/
    }
}
