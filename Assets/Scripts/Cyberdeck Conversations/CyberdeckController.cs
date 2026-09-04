using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    [Header("Phone Chat Integration")]
    [SerializeField] private ChatDialogueView chatDialogueView;

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
            
            dialogueRunner.AddCommandHandler<int>("set_cyberdeck_state", SetCyberdeckState);

            dialogueRunner.AddCommandHandler("enable_cyberdeck_exit", EnableCyberdeckExit);
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

    private void Update()
    {
        bool isCyberdeckOpen = (cyberdeckParentCanvas != null && cyberdeckParentCanvas.activeSelf) || gameObject.activeSelf;
        
        if (!isCyberdeckOpen && !isWaitingForExit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.RequestHurryUpLine();
                }
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

        UpdateChatViewRegistration(); 

        if (stateIndex == 1)
        {
            if (chatDialogueView != null)
            {
                chatDialogueView.ClearChatHistory();
            }

            if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
            {
                LineAdvancer advancer = FindObjectOfType<LineAdvancer>();
                if (advancer != null)
                {
                    advancer.enabled = true;
                    advancer.RequestNextLine();
                }
            }
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
                btn.interactable = (stateIndex != 0 && stateIndex != 1);
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
        
        UpdateChatViewRegistration(); 

        if (index == 1 && messagesNotificationDot != null)
        {
            messagesNotificationDot.SetActive(false);
        }
    }

   private void UpdateChatViewRegistration()
    {
        if (dialogueRunner == null || chatDialogueView == null) return;

        bool isMessageState1Active = (currentStateIndex == 1);

        var presenters = new List<DialoguePresenterBase>(dialogueRunner.DialoguePresenters);

        if (isMessageState1Active)
        {
            if (!presenters.Contains(chatDialogueView))
            {
                presenters.Add(chatDialogueView);
                dialogueRunner.DialoguePresenters = presenters;
            }
            chatDialogueView.enabled = true;
        }
        else
        {
            if (presenters.Contains(chatDialogueView))
            {
                presenters.Remove(chatDialogueView);
                dialogueRunner.DialoguePresenters = presenters;
            }
            chatDialogueView.enabled = false;
        }
    }

    public void OpenMessagesTab()
    {
        selectTab(1);
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
        UpdateChatViewRegistration();
    }

    public void ExitCyberdeck()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        isWaitingForExit = false;

        if (chatDialogueView != null)
        {
            var presenters = new List<DialoguePresenterBase>(dialogueRunner.DialoguePresenters);
            if (presenters.Contains(chatDialogueView))
            {
                presenters.Remove(chatDialogueView);
                dialogueRunner.DialoguePresenters = presenters;
            }
            chatDialogueView.enabled = false;
        }

        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (dialogueRunner != null)
        {
            dialogueRunner.StartCoroutine(ResumeDialogueRoutine());
        }
        else
        {
            HideCyberdeckInstant();
        }
    }

    private IEnumerator ResumeDialogueRoutine()
    {
        yield return null; 

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
    }

    private void HideCyberdeckInstant()
    {
        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (activeLineAdvancers != null)
        {
            foreach (var advancer in activeLineAdvancers)
            {
                if (advancer != null) advancer.enabled = true;
            }
        }

        VisualNovel visualNovel = FindObjectOfType<VisualNovel>();
        if (visualNovel != null)
        {
            visualNovel.PauseDialogue(false);
        }
    }

    public void EnableCyberdeckExit()
    {
        if (exitButton != null)
        {
            Button btn = exitButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = true;
            }
        }
    }

    public void ExitAndLoadScene()
    {
        SceneManager.LoadScene("JenScene");
    }

    public void OpenCyberdeckGeneral()
    {
        VisualNovel visualNovel = FindObjectOfType<VisualNovel>();
        if (visualNovel != null)
        {
            visualNovel.PauseDialogue(true);
        }

        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }

        UpdateChatViewRegistration();
    }

    public void ExitCyberdeckGeneral()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        isWaitingForExit = false;

        if (chatDialogueView != null)
        {
            var presenters = new List<DialoguePresenterBase>(dialogueRunner.DialoguePresenters);
            if (presenters.Contains(chatDialogueView))
            {
                presenters.Remove(chatDialogueView);
                dialogueRunner.DialoguePresenters = presenters;
            }
            chatDialogueView.enabled = false;
        }

        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (dialogueRunner != null)
        {
            dialogueRunner.StartCoroutine(ResumeDialogueRoutine());
        }
        else
        {
            HideCyberdeckInstant();
        }
    }

}
