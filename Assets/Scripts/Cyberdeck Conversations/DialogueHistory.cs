using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueHistory : MonoBehaviour 
{
    [SerializeField] private Transform historyContentContainer;
    [SerializeField] private GameObject historyLinePrefab;
    [SerializeField] private ScrollRect historyScrollRect;

    private void OnEnable()
    {
        Yarn.Unity.LinePresenter.OnLineDelivered += HandleNewDialogue; 
    }

    private void OnDisable()
    {
        Yarn.Unity.LinePresenter.OnLineDelivered -= HandleNewDialogue;
    }

    private void HandleNewDialogue(string characterName, string text)
    {
        string lineText = text;
        
        if (!string.IsNullOrEmpty(characterName))
        {
            string prefix = characterName + ":";
            if (lineText.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
            {
                lineText = lineText.Substring(prefix.Length).TrimStart();
            }

            string hexColor = GetColorForCharacter(characterName);
            lineText = $"<b><color={hexColor}>{characterName.ToUpper()}</color></b>\n{lineText}";    
        }
        /*if (!string.IsNullOrEmpty(characterName) && !text.StartsWith(characterName))
        {
            //lineText = $"<b>{characterName}:</b> {lineText}";
            lineText = $"<b><color=#C87CE8>{characterName.ToUpper()}</color></b>\n{text}";
        }*/

        AddLineToHistory(lineText);
    }

    private void AddLineToHistory(string formattedText)
    {
        if (historyContentContainer == null || historyLinePrefab == null) return;

        GameObject newLine = Instantiate(historyLinePrefab, historyContentContainer);
        TMP_Text tmp = newLine.GetComponentInChildren<TMP_Text>();
        if (tmp != null) tmp.text = formattedText;

        if (historyScrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            historyScrollRect.verticalNormalizedPosition = 0f;
        }
    }

    private string GetColorForCharacter(string charName)
    {
        string nameLower = charName.Trim().ToLower();

        switch (nameLower)
        {
            case "val":
                return "#C87CE8";
            case "luna":
                return "#5BC0EB";
            case "kaya":
                return "#FDE74C";
            default:
                return "#FFFFFF";
        }
    }
}