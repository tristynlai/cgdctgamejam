using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public Canvas SettingsMenu;
    public Canvas CreditsMenu;
    //[SerializeField] int sceneToLoad;
    //[SerializeField] int saveTransferValue;

    void Start()
    {
         if (SettingsMenu != null) {
                SettingsMenu.gameObject.SetActive(false);
         }
         if (CreditsMenu != null) {
                CreditsMenu.gameObject.SetActive(false);
         }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("LoadState", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    /*
    public void LoadGame()
    {
        saveTransferValue = PlayerPrefs.GetInt("LoadState");
        if (saveTransferValue > 0)
        {
            //buttonClick.Play();
            StartCoroutine(LoadScene());
        }
    }
    */

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }

    public void EnterSettingsMenu()
    {
         if (SettingsMenu != null){
                SettingsMenu.gameObject.SetActive(true);
         }
    }

    public void EnterCreditsMenu()
    {
         if (CreditsMenu != null){
                CreditsMenu.gameObject.SetActive(true);
         }
    }

    public void ExitGame()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}