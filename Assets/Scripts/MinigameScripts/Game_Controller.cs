using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Controller : MonoBehaviour
{
    public TextMeshProUGUI HighestTimeText;
    public TextMeshProUGUI CurrentTimeText;
    public TextMeshProUGUI PreviousHighestTimeText;

    public int HighestTime;
    public int CurrentTime;
    public int PreviousHighestTime;

    public Time_Manager Time_Manager;
    public GameObject PauseScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        PauseScreen.SetActive(false);
        PreviousHighestTime = PlayerPrefs.GetInt("HighestTime");
        
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

        int Minutes = Mathf.FloorToInt(PreviousHighestTime / 60);
        int Seconds = Mathf.FloorToInt(PreviousHighestTime % 60);

        PreviousHighestTimeText.text = "Highest Time: " + string.Format("{0:00}:{1:00}", Minutes, Seconds);

    }

    public void QuitGame() {
        //fill out once imported into cyberdeck
    }

    // Update is called once per frame
    void Update() {
        HighestTime = PlayerPrefs.GetInt("HighestTime");
        int Minutes = Mathf.FloorToInt(HighestTime / 60);
        int Seconds = Mathf.FloorToInt(HighestTime % 60);
        HighestTimeText.text = "Highest Time: " + string.Format("{0:00}:{1:00}", Minutes, Seconds);

        CurrentTime = Mathf.FloorToInt(Time_Manager.CurrentTime);
        Minutes = Mathf.FloorToInt(CurrentTime / 60);
        Seconds = Mathf.FloorToInt(CurrentTime % 60);
        CurrentTimeText.text = "Time: " + string.Format("{0:00}:{1:00}", Minutes, Seconds);
    }
}
