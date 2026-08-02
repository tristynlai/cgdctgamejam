using UnityEngine;

[System.Serializable]

public class GameData
{

    public int MinigameHighestTime;
    public string CurrentScene;
    public bool HasSeenIntroCutscene;

    public GameData() {
        this.MinigameHighestTime = 0;
        this.CurrentScene = "";
        this.HasSeenIntroCutscene = false;
    }
}
