using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static string sceneToLoad = "";

    public static void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
    }
}
