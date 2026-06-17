using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject fadeScreenOut;

    public GameObject charGirl;
    [SerializeField] internal YarnProject yarnProject;
    //[YarnNode(nameof(yarnProject))]
    public string startNode = "Start";
    public DialogueRunner DialogueRunner;
    
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
        DialogueRunner.StartDialogue(startNode);
        yield return new WaitForSeconds(2);
        fadeScreenOut.SetActive(true);
        yield return new WaitForSeconds(2);
    }
    
}
