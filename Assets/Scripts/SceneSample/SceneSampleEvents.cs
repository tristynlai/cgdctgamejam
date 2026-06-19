using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject fadeScreenOut;
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
        fadeScreenIn.SetActive(true);
        yield return new WaitForSeconds(2);
        yield return new WaitForSeconds(2);
        //this is where the text box would start
        DialogueRunner.StartDialogue(startNode);
        yield return new WaitUntil(() => variableStorage.TryGetValue("$testVariable", out narrativeOver) == true);
        yield return new WaitForSeconds(2);
        //yield return new WaitForSeconds(2);
        Debug.Log("Narrative over!");
    }

    [YarnCommand("fadeOut")]
    public void FadeOut() {
        Debug.Log("FadeOut CALLED on: " + gameObject.name);
        fadeScreenOut.SetActive(true);
    }

}
