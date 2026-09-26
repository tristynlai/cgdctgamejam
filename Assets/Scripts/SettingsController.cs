using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    public void AutoSaveAndReturnToMenu()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.SaveGame();
            Debug.Log("Game auto-saved before exiting.");
        }
        else
        {
            Debug.LogWarning("SaveManager not found in the scene! Progress was not saved.");
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
