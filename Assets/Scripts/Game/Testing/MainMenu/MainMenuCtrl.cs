using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject mapSelectPanel;

    // Khi bấm nút Play
    public void OpenMapSelect()
    {
        mapSelectPanel.SetActive(true);
    }

    // Khi bấm nút Back trong map select
    public void BackToMain()
    {
        mapSelectPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    // Load map tương ứng
    public void LoadMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Thoát game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game!");
    }
}
