using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject fadeScreenOut;

    public GameObject charGirl;

    public VariableStorageBehaviour variableStorage;

    [SerializeField] internal YarnProject yarnProject;
    [SerializeField] internal bool narrativeOver = false;
    //[YarnNode(nameof(yarnProject))]
    public string startNode = "Start";
    public DialogueRunner DialogueRunner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("LoadState", 1);
        StartCoroutine(EventStarter());
        variableStorage = GameObject.FindAnyObjectByType<InMemoryVariableStorage>();
    }
    
    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2);
        fadeScreenIn.SetActive(false);
        charGirl.SetActive(true);
        yield return new WaitForSeconds(2);
        //this is where the text box would start
        DialogueRunner.StartDialogue(startNode);
        yield return new WaitUntil(() => variableStorage.TryGetValue("$testVariable", out narrativeOver) == true);
        yield return new WaitForSeconds(2);
        fadeScreenOut.SetActive(true);
        yield return new WaitForSeconds(2);
        fadeScreenIn.SetActive(true);
    }

    [YarnCommand("fadeOut")]
    public void FadeOut() {
        Debug.Log("FadeOut CALLED on: " + gameObject.name);
        fadeScreenOut.SetActive(true);
    }
    
    [YarnCommand("fadeIn")]
    public void FadeIn() {
        fadeScreenIn.SetActive(true);
    }

}
