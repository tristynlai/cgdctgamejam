using UnityEngine;
using UnityEngine.Events; 
using Yarn.Unity; 

public class NotificationController : MonoBehaviour
{
    public GameObject notificationGroup;
    public DialogueRunner dialogueRunner;
    public CyberdeckController cyberdeckController; 

     public GameObject alertIndicator;

    [Header("Dialogue Advancer")]
    public LineAdvancer workingAdvancer;

    public UnityEvent onNotificationShown; 
        public UnityEvent onNotificationHidden;

    private int currentNotificationState = -1;

    void Awake()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler<int>("show_notification", ShowNotification);
            dialogueRunner.AddCommandHandler("hide_notification", HideNotification);
        }
    }

    public void ShowNotification(int stateID) 
    {
        currentNotificationState = stateID;

        if (notificationGroup != null)
        {
            notificationGroup.SetActive(true);
                    if (alertIndicator != null) alertIndicator.SetActive(true);
        }

        if (cyberdeckController != null)
        {
            cyberdeckController.SetCyberdeckState(stateID);
            
            if (stateID == 1)
            {
                cyberdeckController.selectTab(1); 
            }
        }

        onNotificationShown.Invoke();
    }

    public void HideNotification()
    {
        if (currentNotificationState == -1) return;

        if (notificationGroup != null) notificationGroup.SetActive(false);
        if (alertIndicator != null) alertIndicator.SetActive(false);

        currentNotificationState = -1;
        onNotificationHidden.Invoke();
    }


    public void AdvanceDialogue()
    {
        if (currentNotificationState == 1)
        {
            if (workingAdvancer != null)
            {
                workingAdvancer.RequestNextLine();
            }
            
            if (notificationGroup != null)
            {
                notificationGroup.SetActive(false);
            }
            
            currentNotificationState = -1;
        }
    }
}
