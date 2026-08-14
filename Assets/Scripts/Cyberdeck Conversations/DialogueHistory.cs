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
            lineText = $"<b>{characterName}:</b> {lineText}";
        }

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
}