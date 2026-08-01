using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Time_Manager : MonoBehaviour, IDataPersistence {
    public float CurrentTime = 0;
    public TextMeshProUGUI TimeText;

    public int HighestTime = 0;
    public static int LastTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        TimeText.text = "0";
        LastTime = HighestTime;
    }

    // Update is called once per frame
    void Update() {
        CurrentTime += Time.deltaTime;

        int Minutes = Mathf.FloorToInt(CurrentTime / 60);
        int Seconds = Mathf.FloorToInt(CurrentTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);

        if (CurrentTime > HighestTime) {
            HighestTime = Mathf.FloorToInt(CurrentTime);
        }
        
    }

    public void ResetTime() {
        CurrentTime = 0f;
        TimeText.text = "00:00";
    }

    public void LoadData(GameData Data) {
        this.HighestTime = Data.MinigameHighestTime;
    }

    public void SaveData(ref GameData Data) {
        Data.MinigameHighestTime = this.HighestTime;
    }
}