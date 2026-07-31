using UnityEngine;
using Yarn.Unity;
using System.Linq;

public class DialogueSwitcher : MonoBehaviour
{
    public GameObject mainDialogueView;
    public GameObject chatDialogueView;
    public DialogueRunner runner;
    private ChatDialogueView chatViewComponent;

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

        if (viewType == "chat")
        {
            if (mainDialogueView != null) mainDialogueView.SetActive(false);
            if (chatDialogueView != null) chatDialogueView.SetActive(true);

            if (chatViewComponent != null)
            {
                var views = runner.DialogueViews.ToList();
                if (!views.Contains(chatViewComponent))
                {
                    views.Add(chatViewComponent);
                    runner.DialogueViews = views.ToArray();
                }
            }
        }
        else if (viewType == "main")
        {
            if (chatViewComponent != null)
            {
                var views = runner.DialogueViews.ToList();
                if (views.Contains(chatViewComponent))
                {
                    views.Remove(chatViewComponent);
                    runner.DialogueViews = views.ToArray();
                }
            }

            if (chatDialogueView != null) chatDialogueView.SetActive(false);
            if (mainDialogueView != null) mainDialogueView.SetActive(true);
        }
    }
}
