using UnityEngine;
using Yarn.Unity;
using System.Linq;

public class DialogueSwitcher : MonoBehaviour
{
    public GameObject mainDialogueView;
    public GameObject chatDialogueView;
    public DialogueRunner runner;
    
    private ChatDialogueView chatViewComponent;
    private DialoguePresenterBase mainViewComponent; 
    private CanvasGroup chatCanvasGroup;

    void Start()
    {
        if (runner != null)
        {
            runner.AddCommandHandler<string>("set_view", SetView);
            runner.onNodeStart.AddListener(HandleNodeStarted);
        }

        if (chatDialogueView != null)
        {
            chatViewComponent = chatDialogueView.GetComponent<ChatDialogueView>();
            chatCanvasGroup = chatDialogueView.GetComponent<CanvasGroup>();
        }

        if (mainDialogueView != null)
        {
            mainViewComponent = mainDialogueView.GetComponent<DialoguePresenterBase>();
        }
    }

    void HandleNodeStarted(string nodeName)
    {
        Debug.Log(">>> CURRENT YARN NODE STARTED: " + nodeName);
    }

    public void SetView(string viewType)
    {
        Debug.Log("SetView command called with type: " + viewType);

        if (runner == null) return;
        
        var views = runner.DialogueViews.ToList();

        if (viewType == "chat")
        {
            if (mainViewComponent != null && views.Contains(mainViewComponent))
            {
                views.Remove(mainViewComponent);
            }

            if (chatCanvasGroup != null)
            {
                chatCanvasGroup.alpha = 1f;
                chatCanvasGroup.interactable = true;
                chatCanvasGroup.blocksRaycasts = true;
            }

            if (chatViewComponent != null && !views.Contains(chatViewComponent))
            {
                views.Add(chatViewComponent);
            }
        }
        else if (viewType == "main")
        {
            if (chatViewComponent != null && views.Contains(chatViewComponent))
            {
                views.Remove(chatViewComponent);
            }

            if (chatCanvasGroup != null)
            {
                chatCanvasGroup.alpha = 0f;
                chatCanvasGroup.interactable = false;
                chatCanvasGroup.blocksRaycasts = false;
            }

            if (mainViewComponent != null && !views.Contains(mainViewComponent))
            {
                views.Add(mainViewComponent);
            }
        }
        
        runner.DialogueViews = views.ToArray();
    }
}
