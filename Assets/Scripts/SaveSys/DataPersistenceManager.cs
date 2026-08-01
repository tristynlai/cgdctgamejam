using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string FileName;

    [SerializeField] private bool UseEncryption;

    private GameData GameData;
    private List<IDataPersistence> DataPersistenceObjects;
    private FileDataHandler DataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one Data Persistence Manager in this scene.");
        }
        instance = this;
    }

    private void Start() {
        this.DataHandler = new FileDataHandler(Application.persistentDataPath, FileName, UseEncryption);
        this.DataPersistenceObjects = FindAllDataPersistenceObjects();

        //remove load game function later
        LoadGame();
    }

    private void OnApplicationQuit() {
        //remove this function later
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> DataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(DataPersistenceObjects);
    }

    public void NewGame() {
        this.GameData = new GameData();
    }

    public void LoadGame() {
        //Load saved data
        this.GameData = DataHandler.Load();

        if (this.GameData == null ) {
            NewGame();
        }

        //push loaded data to all scripts that need it
        foreach (IDataPersistence DataPersistenceObj in DataPersistenceObjects) {
            DataPersistenceObj.LoadData(GameData);
        }

        Debug.Log("Loaded Minigame's Highest Time: " + GameData.MinigameHighestTime);
    }

    public void SaveGame() {
        //pass data to other scripts
        foreach (IDataPersistence DataPersistenceObj in DataPersistenceObjects) {
            DataPersistenceObj.SaveData(ref GameData);
        }

        Debug.Log("Saved Minigame's Highest Time: " + GameData.MinigameHighestTime);

        //save data to file using data handler
        DataHandler.Save(GameData);
    }
}
