using UnityEngine;
using UnityEngine.Events; 
using Yarn.Unity; 

public class NotificationController : MonoBehaviour
{
    public GameObject notificationGroup;
    public DialogueRunner dialogueRunner;
    
    public CyberdeckController cyberdeckController; 

    public UnityEvent onNotificationShown; 

    void Awake()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler<int>("show_notification", ShowNotification);
        }
    }

    public void ShowNotification(int stateID) 
    {
        if (notificationGroup != null)
        {
            notificationGroup.SetActive(true);
        }

        if (cyberdeckController != null)
        {
            cyberdeckController.SetCyberdeckState(stateID);
        }

        onNotificationShown.Invoke();
    }
}