using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CyberdeckController : MonoBehaviour
{
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
    private DialogueRunner? dialogueRunner;

    private bool isWaitingForExit = false;
    private LineAdvancer[] activeLineAdvancers;

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

    private void OnDisable()
    {
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

    public void selectTab(int index)
    {
        for (int i = 0; i < topBarTabs.Length; i++)
        {
            topBarTabs[i].SetActive(i == index);
            pageTabs[i].SetActive(i == index);
        }
        
        if (index == 1 && messagesNotificationDot != null)
        {
            messagesNotificationDot.SetActive(false);
        }
    }

    public void SetCyberdeckState(int stateIndex)
    {
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

    public void ExitCyberdeck()
    {
        StartCoroutine(ExecuteDelayedExit());
    }

    private IEnumerator ExecuteDelayedExit()
    {
        yield return new WaitForSeconds(0.1f);

        if (cyberdeckParentCanvas != null)
        {
            cyberdeckParentCanvas.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (isWaitingForExit)
        {
            isWaitingForExit = false;
        }
    }
}

