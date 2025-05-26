using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject blackOverlay;
    public GameObject hud;
    public GameObject inventory;

    public static bool IsGamePaused { get; private set; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.IsGamePaused)
                FindObjectOfType<PauseMenu>()?.ResumeGame();
            else
                FindObjectOfType<PauseMenu>()?.PauseGame();
        }
    }


    public void PauseGame()
    {
        pausePanel.SetActive(true);
        blackOverlay.SetActive(true);
        hud.SetActive(false);
        inventory.SetActive(false);

        PlayerRotation.allowMouseLook = false;

        Time.timeScale = 0f;
        IsGamePaused = true;
/*        UIStackClose.Push(pausePanel);*/
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        blackOverlay.SetActive(false);
        hud.SetActive(true);
        inventory.SetActive(false);

        PlayerRotation.allowMouseLook = true;

        Time.timeScale = 1f;
        IsGamePaused = false;
/*        UIStackClose.Remove(pausePanel);*/
        Input.ResetInputAxes();
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game!");
    }
}
