using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine.UI;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject fadeScreenOut;
    public GameObject Luna;
    public GameObject Val;
    public VariableStorageBehaviour variableStorage;

    public Texture2D lunaNeutral;

    public AudioSource notificationSource;
    public AudioClip notificationSound;
    public AudioClip tiresSound;

    private Animator lunaAnimator;
    private Animator valAnimator;

    [SerializeField] internal YarnProject yarnProject;
    [SerializeField] internal bool narrativeOver = false;
    //[YarnNode(nameof(yarnProject))]
    public string startNode = "Start";
    public DialogueRunner DialogueRunner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //source = GetComponent<AudioSource>();
        lunaAnimator = Luna.GetComponent<Animator>();
        valAnimator = Val.GetComponent<Animator>();
        PlayerPrefs.SetInt("LoadState", 1);
        StartCoroutine(EventStarter());
        variableStorage = GameObject.FindAnyObjectByType<InMemoryVariableStorage>();
        //DialogueRunner.AddCommandHandler<string>("enter", Enter);
    }
    
    IEnumerator EventStarter()
    {
        DialogueRunner.StartDialogue(startNode);
        yield return new WaitUntil(() => variableStorage.TryGetValue("$testVariable", out narrativeOver) == true);
        yield return new WaitForSeconds(2);
        Debug.Log("Narrative over!");
    }

    [YarnCommand("fadeOut")]
    public void FadeOut() {
        Debug.Log("FadeOut CALLED on: " + gameObject.name);
        fadeScreenOut.SetActive(true);
        fadeScreenIn.SetActive(false);
    }

    [YarnCommand("fadeIn")]
    public void FadeIn() {
        Debug.Log("FadeIn CALLED on: " + gameObject.name);
        fadeScreenIn.SetActive(true);
        fadeScreenOut.SetActive(false);
    }

    [YarnCommand("enter")]
    public void Enter(string character) {
        Debug.Log("Enter CALLED on: " + gameObject.name);
        if (character == "Luna") {
            //lunaAnimator.SetTrigger("FadeIn");
            if (Luna.activeSelf == false) {
                Luna.SetActive(true);
            }
            else
            {
                lunaAnimator.SetTrigger("FadeIn");
            }
        } else if (character == "Val") {
            if (Val.activeSelf == false) {
                Val.SetActive(true);
            }
            else
            {
                valAnimator.SetTrigger("FadeIn");
            }
        }
    }

    [YarnCommand("exit")]
    public void Exit(string character) {
        Debug.Log("Exit CALLED on: " + gameObject.name);
        if (character == "Luna") {
            lunaAnimator.SetTrigger("FadeOut");
            //Luna.SetActive(false);
        } else if (character == "Val") {
            valAnimator.SetTrigger("FadeOut");
            //Val.SetActive(false);
        }
    }

    [YarnCommand("show")]
    public void Show(string character, string expression)
    {
        Debug.Log("Show CALLED on: " + gameObject.name);
        if (character == "Luna")
        {
            Debug.Log($"Luna GameObject: {Luna}");
            
            RawImage lunaImage = Luna.GetComponent<RawImage>();

            Debug.Log($"Image Component: {lunaImage}");
            
            if (expression == "neutral")
            {
                Debug.Log($"Neutral Sprite: {lunaNeutral}");
                lunaImage.texture = lunaNeutral;
            }
        }
    }

    [YarnCommand("sfx")]
    public void SFX(string sfxName)
    {
        Debug.Log("SFX CALLED on: " + gameObject.name);
        if (sfxName == "notification")
        {
            notificationSource.PlayOneShot(notificationSound);
        }
        if (sfxName == "tires")
        {
            notificationSource.PlayOneShot(tiresSound);
        }
    }
}