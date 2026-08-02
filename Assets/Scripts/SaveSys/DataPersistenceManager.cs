using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public bool SaveFileExists() {
        return DataHandler.SaveFileExists();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.DataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence DataPersistenceObj in DataPersistenceObjects) {
            DataPersistenceObj.LoadData(GameData);
        }
    }

    private void Start() {
        this.DataHandler = new FileDataHandler(Application.persistentDataPath, FileName, UseEncryption);
        LoadGame();
    }

    private void OnApplicationQuit() {
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
        this.GameData = DataHandler.Load();

        if (this.GameData == null) {
            NewGame();
        }
    }

    public string GetSavedScene() {
        return string.IsNullOrEmpty(GameData.CurrentScene) ? "SampleScene" : GameData.CurrentScene;
    }

    public void SaveGame() {
        GameData.CurrentScene = SceneManager.GetActiveScene().name;

        this.DataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence DataPersistenceObj in DataPersistenceObjects) {
            DataPersistenceObj.SaveData(ref GameData);
        }

        Debug.Log("Saved Minigame's Highest Time: " + GameData.MinigameHighestTime);

        DataHandler.Save(GameData);
    }
}
