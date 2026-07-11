using UnityEngine;

public class CyberdeckController : MonoBehaviour
{
    public GameObject[] topBarTabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectTab(int index)
    {
        for (int i = 0; i < topBarTabs.Length; i++)
        {
            topBarTabs[i].SetActive(i == index);
        }
    }
}
