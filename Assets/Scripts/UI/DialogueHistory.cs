using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class DialogueHistory : DialoguePresenterBase
{
    [SerializeField] private Transform historyContentContainer;
    [SerializeField] private GameObject historyLinePrefab;
    [SerializeField] private ScrollRect historyScrollRect;

    public override YarnTask RunLineAsync(LocalizedLine dialogueLine, LineCancellationToken cancellationToken)
    {
        string lineText = dialogueLine.Text.Text;
        Debug.Log($"History script received: {lineText}");
        if (!string.IsNullOrEmpty(dialogueLine.CharacterName))
        {
            lineText = $"<b>{dialogueLine.CharacterName}:</b> {lineText}";
        }

        AddLineToHistory(lineText);

        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueStartedAsync()
    {
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        return YarnTask.CompletedTask;
    }

    public override YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] dialogueOptions, LineCancellationToken cancellationToken)
    {
        return YarnTask.FromResult<DialogueOption?>(null);
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