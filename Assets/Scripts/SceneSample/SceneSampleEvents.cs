using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine.UI;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject fadeScreenOut;
    public GameObject frissCity;
    public GameObject alleyway;
    public GameObject Luna;
    public GameObject Val;
    public GameObject Nubs;
    public GameObject Influencer;
    public GameObject Maxx;
    public GameObject Kaya;

    public GameObject Review;

    public VariableStorageBehaviour variableStorage;
    public LineAdvancer lineAdvancer;
    private bool dialoguePaused = false;

    public Sprite lunaNeutral;
    public Sprite lunaAnnoyed;
    public Sprite lunaHappy;
    public Sprite lunaAnnoyedArms;
    public Sprite lunaHappyArms;
    public Sprite lunaShyArms;
    public Sprite lunaNeutralArms;
    public Sprite lunaShyAnnoyed;
    public Sprite lunaShyHappy;
    public Sprite lunaShyNeutral;
    public Sprite lunaShy;

    public Sprite valNeutral;
    public Sprite valSerious;
    public Sprite valFlirty;

    public Sprite podNeutral;
    public Sprite podClosed;
    public Sprite podAngry;
    public Sprite nubsWorried;
    public Sprite nubsWorriedWave;
    public Sprite nubsNeutralWave;
    public Sprite nubsHappyWave;
    public Sprite nubsErrorWave;
    public Sprite nubsConfusedWave;
    public Sprite nubsNeutral;
    public Sprite nubsLoading;
    public Sprite nubsHappy;
    public Sprite nubsError;
    public Sprite nubsConfused;
    public Sprite nubsWorriedArms;
    public Sprite nubsNeutralArms;
    public Sprite nubsHappyArms;
    public Sprite nubsConfusedArms;
    public Sprite nubsAgents;
    public Sprite nubsAngry;
    public Sprite nubsAngryArms;
    public Sprite nubsAngryWave;

    public Sprite influencerNeutral;
    public Sprite influencerHappy;
    public Sprite influencerAnnoyed;

    public Sprite maxxNeutral;
    public Sprite maxxIntrigued;
    public Sprite maxxAnnoyed;

    public Sprite kayaNeutral;
    public Sprite kayaHappy;
    public Sprite kayaAnnoyed;

    public AudioSource notificationSource;
    public AudioSource junkyardSource;
    public AudioClip notificationSound;
    public AudioClip tiresSound;
    public AudioClip engineSound;

    private Animator lunaAnimator;
    private Animator valAnimator;
    private Animator nubsAnimator;
    private Animator influencerAnimator;
    private Animator reviewAnimator;
    private Animator maxxAnimator;
    private Animator kayaAnimator;

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
        nubsAnimator = Nubs.GetComponent<Animator>();
        influencerAnimator = Influencer.GetComponent<Animator>();
        reviewAnimator = Review.GetComponent<Animator>();
        maxxAnimator = Maxx.GetComponent<Animator>();
        kayaAnimator = Kaya.GetComponent<Animator>();

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

    public void ToggleDialoguePause()
    {
        Debug.Log("ToggleDialoguePause CALLED");
        
        dialoguePaused = !dialoguePaused;
        lineAdvancer.enabled = !dialoguePaused;
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
        } else if (character == "Nubs")
        {
            if (Nubs.activeSelf == false)
            {
                Nubs.SetActive(true);
            }
            else
            {
                nubsAnimator.SetTrigger("FadeIn");
            }
        } else if (character == "Review")
        {
            if (Review.activeSelf == false)
            {
                Review.SetActive(true);
            }
            else
            {
                reviewAnimator.SetTrigger("FadeIn");
            }
        } else if (character == "Influencer")
        {
            if (Influencer.activeSelf == false)
            {
                Influencer.SetActive(true);
            }
            else
            {
                influencerAnimator.SetTrigger("FadeIn");
            }
        } else if (character == "Maxx")
        {
            if (Maxx.activeSelf == false)
            {
                Maxx.SetActive(true);
            }
            else
            {
                maxxAnimator.SetTrigger("FadeIn");
            }
        } else if (character == "Kaya")
        {
            if (Kaya.activeSelf == false)
            {
                Kaya.SetActive(true);
            }
            else
            {
                kayaAnimator.SetTrigger("FadeIn");
            }
        }
        
    }

    [YarnCommand("exit")]
    public void Exit(string character) 
    {
        Debug.Log("Exit CALLED on: " + gameObject.name);
        if (character == "Luna") 
        {
            lunaAnimator.SetTrigger("FadeOut");
        } 
        else if (character == "Val") 
        {
            valAnimator.SetTrigger("FadeOut");
        } 
        else if (character == "Nubs")
        {
            nubsAnimator.SetTrigger("FadeOut");
        } 
        else if (character == "Review")
        {
            reviewAnimator.SetTrigger("FadeOut");
        }
        else if (character == "Influencer")
        {
            influencerAnimator.SetTrigger("FadeOut");
        }
        else if (character == "Maxx")
        {
            maxxAnimator.SetTrigger("FadeOut");
        }
        else if (character == "Kaya")
        {
            kayaAnimator.SetTrigger("FadeOut");
        }
    }

    [YarnCommand("show")]
    public void Show(string character, string expression)
    {
        Debug.Log("Show CALLED on: " + gameObject.name);
        if (character == "Luna")
        {
            Debug.Log($"Luna GameObject: {Luna}");
            
            Image lunaImage = Luna.GetComponent<Image>();

            Debug.Log($"Image Component: {lunaImage}");
            
            if (expression == "lunaNeutral")
            {
                Debug.Log($"Neutral Sprite: {lunaNeutral}");
                lunaImage.sprite = lunaNeutral;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaAnnoyed")
            {
                Debug.Log($"Annoyed Sprite: {lunaAnnoyed}");
                lunaImage.sprite = lunaAnnoyed;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaHappy")
            {
                Debug.Log($"Happy Sprite: {lunaHappy}");
                lunaImage.sprite = lunaHappy;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaAnnoyedArms")
            {
                Debug.Log($"Annoyed Arms Sprite: {lunaAnnoyedArms}");
                lunaImage.sprite = lunaAnnoyedArms;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaHappyArms")
            {
                Debug.Log($"Happy Arms Sprite: {lunaHappyArms}");
                lunaImage.sprite = lunaHappyArms;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaShyArms")
            {
                Debug.Log($"Shy Arms Sprite: {lunaShyArms}");
                lunaImage.sprite = lunaShyArms;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaNeutralArms")
            {
                Debug.Log($"Neutral Arms Sprite: {lunaNeutralArms}");
                lunaImage.sprite = lunaNeutralArms;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaShyAnnoyed")
            {
                Debug.Log($"Shy Annoyed Sprite: {lunaShyAnnoyed}");
                lunaImage.sprite = lunaShyAnnoyed;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaShyHappy")
            {
                Debug.Log($"Shy Happy Sprite: {lunaShyHappy}");
                lunaImage.sprite = lunaShyHappy;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaShyNeutral")
            {
                Debug.Log($"Shy Neutral Sprite: {lunaShyNeutral}");
                lunaImage.sprite = lunaShyNeutral;
                lunaImage.SetNativeSize();
            }
            else if (expression == "lunaShy")
            {
                Debug.Log($"Shy Sprite: {lunaShy}");
                lunaImage.sprite = lunaShy;
                lunaImage.SetNativeSize();
            }
        }
        else if (character == "Val")
        {
            Debug.Log($"Val GameObject: {Val}");
            
            Image valImage = Val.GetComponent<Image>();

            Debug.Log($"Image Component: {valImage}");
            
            if (expression == "valNeutral")
            {
                Debug.Log($"Neutral Sprite: {valNeutral}");
                valImage.sprite = valNeutral;
                valImage.SetNativeSize();
            }
            else if (expression == "valSerious")
            {
                Debug.Log($"Serious Sprite: {valSerious}");
                valImage.sprite = valSerious;
                valImage.SetNativeSize();
            }
            else if (expression == "valFlirty")
            {
                Debug.Log($"Flirty Sprite: {valFlirty}");
                valImage.sprite = valFlirty;
                valImage.SetNativeSize();
            }
        }
        else if (character == "Nubs")
        {
            Debug.Log($"Nubs GameObject: {Nubs}");
            
            //RawImage nubsImage = Nubs.GetComponent<RawImage>();
            Image nubsImage = Nubs.GetComponent<Image>();

            Debug.Log($"Image Component: {nubsImage}");
            
            if (expression == "podNeutral")
            {
                Debug.Log($"Pod Neutral Sprite: {podNeutral}");
                //nubsImage.texture = nubsNeutral;
                nubsImage.sprite = podNeutral;
                nubsImage.SetNativeSize();
            }
            else if (expression == "podClosed")
            {
                Debug.Log($"Pod Closed Sprite: {podClosed}");
                nubsImage.sprite = podClosed;
                nubsImage.SetNativeSize();
            }
            else if (expression == "podAngry")
            {
                Debug.Log($"Pod Angry Sprite: {podAngry}");
                nubsImage.sprite = podAngry;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsWorried")
            {
                Debug.Log($"Nubs Worried Sprite: {nubsWorried}");
                nubsImage.sprite = nubsWorried;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsWorriedWave")
            {
                Debug.Log($"Nubs Worried Wave Sprite: {nubsWorriedWave}");
                nubsImage.sprite = nubsWorriedWave;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsNeutralWave")
            {
                Debug.Log($"Nubs Neutral Wave Sprite: {nubsNeutralWave}");
                nubsImage.sprite = nubsNeutralWave;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsHappyWave")
            {
                Debug.Log($"Nubs Happy Wave Sprite: {nubsHappyWave}");
                nubsImage.sprite = nubsHappyWave;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsErrorWave")
            {
                Debug.Log($"Nubs Error Wave Sprite: {nubsErrorWave}");
                nubsImage.sprite = nubsErrorWave;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsConfusedWave")
            {
                Debug.Log($"Nubs Confused Wave Sprite: {nubsConfusedWave}");
                nubsImage.sprite = nubsConfusedWave;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsNeutral")
            {
                Debug.Log($"Nubs Neutral Sprite: {nubsNeutral}");
                nubsImage.sprite = nubsNeutral;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsLoading")
            {
                Debug.Log($"Nubs Loading Sprite: {nubsLoading}");
                nubsImage.sprite = nubsLoading;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsHappy")
            {
                Debug.Log($"Nubs Happy Sprite: {nubsHappy}");
                nubsImage.sprite = nubsHappy;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsError")
            {
                Debug.Log($"Nubs Error Sprite: {nubsError}");
                nubsImage.sprite = nubsError;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsConfused")
            {
                Debug.Log($"Nubs Confused Sprite: {nubsConfused}");
                nubsImage.sprite = nubsConfused;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsWorriedArms")
            {
                Debug.Log($"Nubs Worried Arms Sprite: {nubsWorriedArms}");
                nubsImage.sprite = nubsWorriedArms;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsNeutralArms")
            {
                Debug.Log($"Nubs Neutral Arms Sprite: {nubsNeutralArms}");
                nubsImage.sprite = nubsNeutralArms;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsHappyArms")
            {
                Debug.Log($"Nubs Happy Arms Sprite: {nubsHappyArms}");
                nubsImage.sprite = nubsHappyArms;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsConfusedArms")
            {
                Debug.Log($"Nubs Confused Arms Sprite: {nubsConfusedArms}");
                nubsImage.sprite = nubsConfusedArms;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsAgents")
            {
                Debug.Log($"Nubs Agents Sprite: {nubsAgents}");
                nubsImage.sprite = nubsAgents;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsAngry")
            {
                Debug.Log($"Nubs Angry Sprite: {nubsAngry}");
                nubsImage.sprite = nubsAngry;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsAngryArms")
            {
                Debug.Log($"Nubs Angry Arms Sprite: {nubsAngryArms}");
                nubsImage.sprite = nubsAngryArms;
                nubsImage.SetNativeSize();
            }
            else if (expression == "nubsAngryWave")
            {
                Debug.Log($"Nubs Angry Wave Sprite: {nubsAngryWave}");
                nubsImage.sprite = nubsAngryWave;
                nubsImage.SetNativeSize();
            }
        }
        else if (character == "Influencer")
        {
            Debug.Log($"Influencer GameObject: {Influencer}");
            
            Image influencerImage = Influencer.GetComponent<Image>();

            Debug.Log($"Image Component: {influencerImage}");
            
            if (expression == "influencerNeutral")
            {
                Debug.Log($"Influencer Neutral Sprite: {influencerNeutral}");
                influencerImage.sprite = influencerNeutral;
                influencerImage.SetNativeSize();
            }
            else if (expression == "influencerHappy")
            {
                Debug.Log($"Influencer Happy Sprite: {influencerHappy}");
                influencerImage.sprite = influencerHappy;
                influencerImage.SetNativeSize();
            }
            else if (expression == "influencerAnnoyed")
            {
                Debug.Log($"Influencer Annoyed Sprite: {influencerAnnoyed}");
                influencerImage.sprite = influencerAnnoyed;
                influencerImage.SetNativeSize();
            }
        }
        else if (character == "Maxx")
        {
            Debug.Log($"Maxx GameObject: {Maxx}");
            
            Image maxxImage = Maxx.GetComponent<Image>();

            Debug.Log($"Image Component: {maxxImage}");
            
            if (expression == "maxxNeutral")
            {
                Debug.Log($"Maxx Neutral Sprite: {maxxNeutral}");
                maxxImage.sprite = maxxNeutral;
                maxxImage.SetNativeSize();
            }
            else if (expression == "maxxIntrigued")
            {
                Debug.Log($"Maxx Intrigued Sprite: {maxxIntrigued}");
                maxxImage.sprite = maxxIntrigued;
                maxxImage.SetNativeSize();
            }
            else if (expression == "maxxAnnoyed")
            {
                Debug.Log($"Maxx Annoyed Sprite: {maxxAnnoyed}");
                maxxImage.sprite = maxxAnnoyed;
                maxxImage.SetNativeSize();
            }
        }
        else if (character == "Kaya")
        {
            Debug.Log($"Kaya GameObject: {Kaya}");
            
            Image kayaImage = Kaya.GetComponent<Image>();

            Debug.Log($"Image Component: {kayaImage}");
            
            if (expression == "kayaNeutral")
            {
                Debug.Log($"Kaya Neutral Sprite: {kayaNeutral}");
                kayaImage.sprite = kayaNeutral;
                kayaImage.SetNativeSize();
            }
            else if (expression == "kayaHappy")
            {
                Debug.Log($"Kaya Happy Sprite: {kayaHappy}");
                kayaImage.sprite = kayaHappy;
                kayaImage.SetNativeSize();
            }
            else if (expression == "kayaAnnoyed")
            {
                Debug.Log($"Kaya Annoyed Sprite: {kayaAnnoyed}");
                kayaImage.sprite = kayaAnnoyed;
                kayaImage.SetNativeSize();
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
        if (sfxName == "engine")
        {
            notificationSource.PlayOneShot(engineSound);
        }
    }

    [YarnCommand("play")]
    public void Play(string audioName)
    {
        Debug.Log("Play CALLED on: " + gameObject.name);
        if (audioName == "junkyard")
        {
            junkyardSource.Play();
        }
    }

    [YarnCommand("stop")]
    public void Stop(string audioName)
    {
        Debug.Log("Stop CALLED on: " + gameObject.name);
        if (audioName == "junkyard")
        {
            junkyardSource.Stop();
        }
    }

    [YarnCommand("background")]
    public void Background(string backgroundName)
    {
        Debug.Log("Background CALLED on: " + gameObject.name);
        if (backgroundName == "frissCity")
        {
            frissCity.SetActive(true);
        }
        else if (backgroundName == "alleyway")
        {
            alleyway.SetActive(true);
        }
    }
}