using UnityEngine;
using UnityEngine.Events; 
using Yarn.Unity; 

public class NotificationController : MonoBehaviour
{
    public GameObject notificationGroup;
    public DialogueRunner dialogueRunner;
    public UnityEvent onNotificationShown; 

    void Awake()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("show_notification", ShowNotification);
        }
    }

    public void ShowNotification()
    {
        if (notificationGroup != null)
        {
            notificationGroup.SetActive(true);
        }
        onNotificationShown.Invoke();
    }
}