using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public string savedScene;
    public string savedNode;

    public List<string> floatKeys = new List<string>();
    public List<float> floatValues = new List<float>();

    public List<string> stringKeys = new List<string>();
    public List<string> stringValues = new List<string>();

    public List<string> boolKeys = new List<string>();
    public List<bool> boolValues = new List<bool>();
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    private const string SAVE_KEY = "CyberdeckGameSave";

    [Header("Save Checkpoints")]
    public List<string> majorCheckpointNodes = new List<string>();

    private string currentYarnNode = "";
    private string lastPassedCheckpoint = "";
    
    private static bool isRestoringSave = false;
    private static GameSaveData pendingSaveData = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (dialogueRunner != null)
        {
            dialogueRunner.onNodeStart.AddListener(UpdateCurrentNode);
            dialogueRunner.onDialogueComplete.AddListener(ClearCurrentNode);

            if (isRestoringSave && pendingSaveData != null)
            {
                InMemoryVariableStorage varStorage = dialogueRunner.VariableStorage as InMemoryVariableStorage;
                if (varStorage != null)
                {
                    varStorage.Clear(); 
                    for (int i = 0; i < pendingSaveData.floatKeys.Count; i++) varStorage.SetValue(pendingSaveData.floatKeys[i], pendingSaveData.floatValues[i]);
                    for (int i = 0; i < pendingSaveData.stringKeys.Count; i++) varStorage.SetValue(pendingSaveData.stringKeys[i], pendingSaveData.stringValues[i]);
                    for (int i = 0; i < pendingSaveData.boolKeys.Count; i++) varStorage.SetValue(pendingSaveData.boolKeys[i], pendingSaveData.boolValues[i]);
                }

                if (!string.IsNullOrEmpty(pendingSaveData.savedNode))
                {
                    dialogueRunner.startNode = pendingSaveData.savedNode;
                }

                UnityEngine.Video.VideoPlayer videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();
                if (videoPlayer != null)
                {
                    videoPlayer.Stop();
                    videoPlayer.gameObject.SetActive(false);
                }

                StartCoroutine(EnsureDialogueStarts(dialogueRunner, pendingSaveData.savedNode));
            }
        }
    }

    private IEnumerator EnsureDialogueStarts(DialogueRunner runner, string savedNode)
    {
        for (int i = 0; i < 3; i++)
        {
            Input.ResetInputAxes();
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
            yield return new WaitForEndOfFrame();
        }

        if (runner != null && !runner.IsDialogueRunning && !string.IsNullOrEmpty(savedNode))
        {
            Debug.Log($"[SaveManager] Manual start triggered for node: {savedNode}");
            runner.StartDialogue(savedNode);
        }

        isRestoringSave = false;
        pendingSaveData = null;
    }

    private void UpdateCurrentNode(string nodeName)
    {
        currentYarnNode = nodeName;

        if (majorCheckpointNodes.Contains(nodeName))
        {
            lastPassedCheckpoint = nodeName;
        }
    }

    private void ClearCurrentNode()
    {
        currentYarnNode = "";
        lastPassedCheckpoint = "";
    }

    public void SaveGame()
    {
        GameSaveData data = new GameSaveData();
        data.savedScene = SceneManager.GetActiveScene().name;

        DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (dialogueRunner != null)
        {
            if (dialogueRunner.IsDialogueRunning)
            {
                data.savedNode = !string.IsNullOrEmpty(lastPassedCheckpoint) ? lastPassedCheckpoint : currentYarnNode;
                
                if (string.IsNullOrEmpty(data.savedNode))
                {
                    data.savedNode = dialogueRunner.startNode;
                }
            }

            Debug.Log($"[SaveManager] Saving Node: {data.savedNode} in Scene: {data.savedScene}");

            InMemoryVariableStorage varStorage = dialogueRunner.VariableStorage as InMemoryVariableStorage;
            if (varStorage != null)
            {
                var (floatVars, stringVars, boolVars) = varStorage.GetAllVariables();
                
                foreach (var kvp in floatVars) { data.floatKeys.Add(kvp.Key); data.floatValues.Add(kvp.Value); }
                foreach (var kvp in stringVars) { data.stringKeys.Add(kvp.Key); data.stringValues.Add(kvp.Value); }
                foreach (var kvp in boolVars) { data.boolKeys.Add(kvp.Key); data.boolValues.Add(kvp.Value); }
            }
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        
        Debug.Log("[SaveManager] Game Saved Successfully!");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            pendingSaveData = JsonUtility.FromJson<GameSaveData>(json);
            
            isRestoringSave = true; 
            SceneManager.LoadSceneAsync(pendingSaveData.savedScene);
        }
        else
        {
            Debug.LogWarning("[SaveManager] No save data found!");
        }
    }

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
}
