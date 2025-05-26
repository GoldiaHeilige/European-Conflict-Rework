/*using UnityEngine;

public class UIEscManager : MonoBehaviour
{
    public GameObject pausePanel; 
    public GameObject blackOverlay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIStackClose.HasPopup)
            {
                UIStackClose.PopTop();

                if (!UIStackClose.HasPopup && !PauseMenu.IsGamePaused)
                {
                    FindObjectOfType<PauseMenu>()?.PauseGame();
                }
            }
            else
            {
                if (PauseMenu.IsGamePaused)
                    FindObjectOfType<PauseMenu>()?.ResumeGame();
                else
                    FindObjectOfType<PauseMenu>()?.PauseGame();
            }
        }
    }

}
*/