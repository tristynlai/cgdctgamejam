using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int sceneToLoad;
    [SerializeField] int saveTransferValue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    /*
    public void StartGame()
    {
        buttonClick.Play();
        PlayerPrefs.SetInt("LoadState", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    */
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

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
    */

}
