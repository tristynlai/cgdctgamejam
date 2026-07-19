using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Controller : MonoBehaviour
{
    public TextMeshProUGUI HighestTimeText;
    public TextMeshProUGUI CurrentTimeText;

    public int HighestTime;
    public int CurrentTime;

    public Time_Manager Time_Manager;
    public GameObject PauseScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        PauseScreen.SetActive(false);
        
    }

    public void RestartGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CyberdeckGame");
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        PauseScreen.SetActive(false);
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        PauseScreen.SetActive(true);

    }

    public void QuitGame() {
        //fill out once imported into cyberdeck
    }

    // Update is called once per frame
    void Update() {
        HighestTime = PlayerPrefs.GetInt("HighestTime");
        CurrentTime = Mathf.FloorToInt(Time_Manager.CurrentTime);

        HighestTimeText.text = "Highest Time: " + HighestTime.ToString();
        CurrentTimeText.text = "Time: " + CurrentTime.ToString();
    }
}
