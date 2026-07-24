using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance;

    public TextMeshProUGUI HighestTimeText;
    public TextMeshProUGUI CurrentTimeText;
    public TextMeshProUGUI PreviousHighestTimeText;

    public int HighestTime;
    public int CurrentTime;
    public int PreviousHighestTime;

    public Time_Manager Time_Manager;
    public Player_Movement Player_Movement;
    public Car_Spawner Car_Spawner;
    public Road_Movement Road_Movement;

    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject StartScreen;
    public GameObject TutorialScreen;

    public AudioSource Music;

    void Awake() {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        PauseScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        PreviousHighestTime = PlayerPrefs.GetInt("HighestTime");

        Time.timeScale = 0f;

        StartScreen.SetActive(true);
        TutorialScreen.SetActive(false);
    }
    
    public void StartPressed() {
        StartScreen.SetActive(false);
        BeginGame();
    }

    public void TutorialPressed() {
        StartScreen.SetActive(false);
        TutorialScreen.SetActive(true);
    }

    public void TutorialStartPressed() {
        TutorialScreen.SetActive(false);
        BeginGame();
    }

    public void RestartGame() {
        Music.Play();
        GameOverScreen.SetActive(false);
        Time.timeScale = 1f;

        if (Time_Manager != null) {
            Time_Manager.ResetTime();
        }

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Cars");
        foreach (GameObject car in cars) {
            Destroy(car);
        }

        Car_Movement.Speed = 4f;
        if (Car_Spawner != null) {
            Car_Spawner.ResetSpawner();
        }

        if (Road_Movement != null) {
            Road_Movement.ResetSpeed();
        }

        if (Player_Movement != null) {
            Player_Movement.ResetPlayer();
        }
    }

    public void GameOver() {
        Time.timeScale = 0f;
        GameOverScreen.SetActive(true);
        Music.Stop();
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        PauseScreen.SetActive(false);
        Music.UnPause();
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        
        PauseScreen.SetActive(true);

        int Minutes = Mathf.FloorToInt(PreviousHighestTime / 60);
        int Seconds = Mathf.FloorToInt(PreviousHighestTime % 60);

        Music.Pause();

        PreviousHighestTimeText.text = "Highest Time: " + string.Format("{0:00}:{1:00}", Minutes, Seconds);

    }

    private void BeginGame() {
        Music.Play();
        Time.timeScale = 1f;
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
