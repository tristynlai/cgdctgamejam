using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameMenuManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void ExitLoadGameMenu()
    {
        gameObject.SetActive(false);
    }

    public void NewGame()
    {
         Debug.Log("NewGame called - attempting to load IntroScene");
        PlayerPrefs.SetInt("LoadState", 0);
         UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("LoadState", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
