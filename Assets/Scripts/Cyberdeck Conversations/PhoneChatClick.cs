using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class PhoneChatClick : MonoBehaviour, IPointerClickHandler
{
    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.RequestNextLine();
        }
    }
}
