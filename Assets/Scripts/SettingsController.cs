using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SettingsController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    public void AutoSaveAndReturnToMenu()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.SaveAndQuitToMainMenu();
            Debug.Log("Game saved and safely returned to main menu.");
        }
        else
        {
            Debug.LogWarning("SaveManager not found in the scene! Forcing direct scene load.");
            
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop();
            }

            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}