using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public Canvas SettingsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         if (SettingsMenu != null) {
                SettingsMenu.gameObject.SetActive(false);
         }
    }

    public void EnterSettingsMenu()
    {
         if (SettingsMenu != null){
                SettingsMenu.gameObject.SetActive(true);
         }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}