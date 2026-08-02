using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;

[System.Serializable]
public struct CharacterNameplate
{
    public string characterName;
    public Sprite nameplateSprite;
}

public class NameplateSwitch : MonoBehaviour
{
    public TextMeshProUGUI yarnNameText; 

    [Header("UI References")]
    public GameObject nameplateObject; 
    public Image nameplateImage;       

    [Header("Character Sprites")]
    public CharacterNameplate[] nameplates; 

    private string lastSeenName = "INITIALIZE_ON_START"; 

    void Update()
    {
        if (yarnNameText == null) return;

        string currentText;

        if (yarnNameText.gameObject.activeInHierarchy == false)
        {
            currentText = "Luna";
        }
        else
        {
            currentText = yarnNameText.text.Trim();
            
            if (string.IsNullOrEmpty(currentText)) 
            {
                currentText = "Luna"; 
            }
        }

        if (currentText == lastSeenName) return;

        lastSeenName = currentText;
        UpdateSprite(lastSeenName);
    }

    private void UpdateSprite(string currentName)
    {
        if (nameplateObject != null) nameplateObject.SetActive(true);

        if (nameplateImage != null)
        {
            bool foundMatch = false;
            foreach (var np in nameplates)
            {
                if (string.Equals(np.characterName.Trim(), currentName, StringComparison.OrdinalIgnoreCase))
                {
                    nameplateImage.sprite = np.nameplateSprite;
                    foundMatch = true;
                    break; 
                }
            }
            
            if (!foundMatch) 
            {
                Debug.LogWarning($"Could not find a matching sprite for: '{currentName}'");
            }
        }
    }
}