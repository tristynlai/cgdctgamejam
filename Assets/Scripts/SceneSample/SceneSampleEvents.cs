using UnityEngine;
using Systems.Collections;
//using Systems.Collections.Generic;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject charGirl;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("LoadState", 1);
        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2);
        fadeScreenIn.SetActive(false);
        charGirl.SetActive(true);
        yield return new WaitForSeconds(2);
        //this is where the text box would start
        yield return new WaitForSeconds(2);

    }
}
