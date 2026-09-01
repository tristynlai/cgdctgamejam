using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.EventSystems;

public class CyberdeckController : MonoBehaviour
{
    [Header("UI Tabs & Pages")]
    public GameObject[] topBarTabs;
    public GameObject[] pageTabs;
    
    [Header("Narrative States")]
    public GameObject[] homeStates;
    public GameObject[] messageStates;

    [Header("UI Indicators")]
    public GameObject messagesNotificationDot;
    
    [Header("Buttons")]
    public GameObject exitButton;

    [Header("References")]
    [SerializeField] private GameObject cyberdeckParentCanvas;
    private DialogueRunner dialogueRunner;

    private bool isWaitingForExit = false;
    private LineAdvancer[] activeLineAdvancers;

    private int currentStateIndex = 0;

    private void Awake()
    {
        dialogueRunner = DialogueRunner.FindRunner(this);
        
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("wait_for_cyberdeck_exit", WaitForCyberdeckExit);
        }
    }

    private void OnEnable()
    {
        activeLineAdvancers = FindObjectsByType<LineAdvancer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        
        foreach (var advancer in activeLineAdvancers)
        {
            if (advancer != null)
            {
                advancer.enabled = false;
            }
        }
    }

    private IEnumerator WaitForCyberdeckExit()
    {
        isWaitingForExit = true;
        
        if (exitButton != null)
        {
            Button btn = exitButton.GetComponent<Button>();
            if (btn != null) btn.interactable = true;
        }

        while (isWaitingForExit)
        {
            yield return null;
        }
    }

    public void SetCyberdeckState(int stateIndex)
    {
        currentStateIndex = stateIndex;

        for (int i = 0; i < homeStates.Length; i++)
        {
            if (homeStates[i] != null) homeStates[i].SetActive(i == stateIndex);
        }

        for (int i = 0; i < messageStates.Length; i++)
        {
            if (messageStates[i] != null) messageStates[i].SetActive(i == stateIndex);
        }

        if (messagesNotificationDot != null)
        {
            messagesNotificationDot.SetActive(true);
        }
        
        if (exitButton != null)
        {
            Button btn = exitButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = (stateIndex != 1);
            }
        }
    }

    public void selectTab(int index)
    {
        for (int i = 0; i < topBarTabs.Length; i++)
        {
            topBarTabs[i].SetActive(i == index);
            pageTabs[i].SetActive(i == index);
        }
        
        if (index == 1)
        {
            for (int i = 0; i < messageStates.Length; i++)
            {
                if (messageStates[i] != null) 
                {
                    messageStates[i].SetActive(i == currentStateIndex);
                }
            }
        }
        
        if (index == 1 && messagesNotificationDot != null)
        {
            messagesNotificationDot.SetActive(false);
        }
    }

    public void OpenMessagesTab()
    {
        selectTab(1);
    }

    public void ExitCyberdeck()
    {
        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (activeLineAdvancers != null)
        {
            foreach (var advancer in activeLineAdvancers)
            {
                if (advancer != null)
                {
                    advancer.enabled = true;
                }
            }
        }

        VisualNovel visualNovel = FindObjectOfType<VisualNovel>();
        if (visualNovel != null)
        {
            visualNovel.PauseDialogue(false);
        }

        isWaitingForExit = false;
    }

    public void OpenCyberdeck()
    {
        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

