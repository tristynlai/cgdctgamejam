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

    void LateUpdate()
    {
        if (yarnNameText == null) return;

        if (nameplateObject != null && !nameplateObject.activeSelf)
        {
            nameplateObject.SetActive(true);
        }

        if (!yarnNameText.gameObject.activeSelf)
        {
            yarnNameText.gameObject.SetActive(true);
        }

        string currentText = yarnNameText.text.Trim();

        if (string.IsNullOrEmpty(currentText))
        {
            currentText = "Luna";
            yarnNameText.text = "LUNA";
        }

        if (currentText == lastSeenName) return;

        lastSeenName = currentText;
        UpdateSprite(lastSeenName);
    }

    private void UpdateSprite(string currentName)
    {
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