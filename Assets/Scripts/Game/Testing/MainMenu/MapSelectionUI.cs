using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSelectionUI : MonoBehaviour
{
    [Header("Confirm Popup")]
    public GameObject confirmPopup; 
    public TMP_Text contentText;   
    private string selectedSceneName;

    public void OnClickMapButton(string sceneName)
    {
        selectedSceneName = sceneName;

        // Cập nhật nội dung popup
        contentText.text = sceneName;

        // Hiện popup
        confirmPopup.SetActive(true);
    }

    public void OnConfirmYes()
    {
        SceneLoader.LoadScene(selectedSceneName);
    }

    public void OnConfirmCancel()
    {
        confirmPopup.SetActive(false);
    }
}
