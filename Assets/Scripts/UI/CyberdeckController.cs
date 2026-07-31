using UnityEngine;

public class CyberdeckController : MonoBehaviour
{
    public GameObject[] topBarTabs;
    public GameObject[] pageTabs;
    
    [Header("Narrative States")]
    public GameObject[] homeStates;
    
    public GameObject[] messageStates;

    [Header("UI Indicators")]
    public GameObject messagesNotificationDot;

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
    }
}
