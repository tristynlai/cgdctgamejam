using UnityEngine;
using Yarn.Unity;

public class DialogueSwitcher : MonoBehaviour
{
    public GameObject mainDialogueView;
    
    public GameObject chatDialogueView;
    
    public DialogueRunner runner;

    void Awake()
    {
        if (runner != null)
        {
            runner.AddCommandHandler<string>("set_view", SetView);
        }
    }

    public void SetView(string viewType)
    {
        if (viewType == "chat")
        {
            mainDialogueView.SetActive(false);
            chatDialogueView.SetActive(true);
        }
        else if (viewType == "main")
        {
            chatDialogueView.SetActive(false);
            mainDialogueView.SetActive(true);
        }
    }
}
