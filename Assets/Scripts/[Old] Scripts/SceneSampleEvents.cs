using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine.UI;

public class SceneSampleEvents : MonoBehaviour
{
    public GameObject frissCity;
    public GameObject alleyway;
    public GameObject lilguyCG;
    public GameObject cybergullsCG;
    public GameObject Luna;
    public GameObject LunaOnBike;
    public GameObject Val;
    public GameObject Pod;
    public GameObject Nubs;
    public GameObject Influencer;
    public GameObject Maxx;
    public GameObject Kaya;
    public GameObject Nat;
    public GameObject Cybergull;
    public GameObject Dad;
    public GameObject lunaBike;
    public GameObject maxxBike;
    public GameObject valBike;

    public GameObject Review;

    public VariableStorageBehaviour variableStorage;
    public LineAdvancer lineAdvancer;
    private bool dialoguePaused = false;

    public Sprite lunaNeutral;
    public Sprite lunaAnnoyed;
    public Sprite lunaHappy;
    public Sprite lunaSurprised;
    public Sprite lunaAnnoyedArms;
    public Sprite lunaHappyArms;
    public Sprite lunaShyArms;
    public Sprite lunaSurprisedArms;
    public Sprite lunaNeutralArms;
    public Sprite lunaShyAnnoyed;
    public Sprite lunaShyHappy;
    public Sprite lunaShyNeutral;
    public Sprite lunaShySurprised;
    public Sprite lunaShy;
    public Sprite lunaAngryOnBike;
    public Sprite lunaAnnoyedOnBike;
    public Sprite lunaNeutralOnBike;
    public Sprite lunaSurprisedOnBike;

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
    public Sprite nubsAngryEars;
    public Sprite nubsNeutralEars;
    public Sprite nubsHappyEars;
    public Sprite nubsErrorEars;

    public Sprite influencerNeutral;
    public Sprite influencerHappy;
    public Sprite influencerAnnoyed;

    public Sprite maxxNeutral;
    public Sprite maxxIntrigued;
    public Sprite maxxAnnoyed;

    public Sprite kayaNeutral;
    public Sprite kayaHappy;
    public Sprite kayaAnnoyed;

    public Sprite natNeutral;
    public Sprite natCute;
    public Sprite natScared;

    public Sprite cybergullCog;
    public Sprite cybergullCogless;
    public Sprite cybergullFreed;

    public Sprite dadNeutral;
    public Sprite dadHappy;
    public Sprite dadAnnoyed;

    public AudioSource notificationSource;
    public AudioSource frisscitySource;
    public AudioSource luxapt02Source;
    public AudioSource sundayneonsSource;
    public AudioSource junkyardSource;
    public AudioSource deadroomSource;
    public AudioSource ascensionmellowSource;
    public AudioSource loungelizardsSource;
    public AudioSource undergroundlabSource;
    public AudioSource errorofourwaysSource;
    public AudioClip notificationSound;
    public AudioClip tiresSound;
    public AudioClip engineSound;
    public AudioClip podOpen;
    public AudioClip shortgull;
    public AudioClip longgull;
    public AudioClip beep;
    public AudioClip maxxMotorcycle;
    public AudioClip gullBattle;
    public AudioClip metalGrinding;

    private Animator lunaAnimator;
    private Animator valAnimator;
    private Animator podAnimator;
    private Animator nubsAnimator;
    private Animator influencerAnimator;
    private Animator reviewAnimator;
    private Animator maxxAnimator;
    private Animator kayaAnimator;
    private Animator natAnimator;
    private Animator cybergullAnimator;
    private Animator dadAnimator;

    //public GameObject cyberdeck;

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
        podAnimator = Pod.GetComponent<Animator>();
        nubsAnimator = Nubs.GetComponent<Animator>();
        influencerAnimator = Influencer.GetComponent<Animator>();
        reviewAnimator = Review.GetComponent<Animator>();
        maxxAnimator = Maxx.GetComponent<Animator>();
        kayaAnimator = Kaya.GetComponent<Animator>();
        natAnimator = Nat.GetComponent<Animator>();
        cybergullAnimator = Cybergull.GetComponent<Animator>();
        dadAnimator = Dad.GetComponent<Animator>();

        PlayerPrefs.SetInt("LoadState", 1);
        //StartCoroutine(EventStarter());
        variableStorage = GameObject.FindAnyObjectByType<InMemoryVariableStorage>();
        StartCoroutine(EventStarter());

        //DialogueRunner.AddCommandHandler<string>("enter", Enter);
    }
    
    IEnumerator EventStarter()
    {
        DialogueRunner.StartDialogue(startNode);
        yield return new WaitUntil(() => variableStorage.TryGetValue("$testVariable", out narrativeOver) == true);
        yield return new WaitForSeconds(2);
        Debug.Log("Narrative over!");
    }

    public void SetDialoguePause(bool isPaused)
    {
        Debug.Log("Forcing Dialogue Pause State to: " + isPaused);
        dialoguePaused = isPaused;
        
        if (lineAdvancer != null)
        {
            lineAdvancer.enabled = !dialoguePaused;
        }
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
        } else if (character == "Pod")
        {
            if (Pod.activeSelf == false)
            {
                Pod.SetActive(true);
            }
            else
            {
                podAnimator.SetTrigger("FadeIn");
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
        } else if (character == "Nat")
        {
            if (Nat.activeSelf == false)
            {
                Nat.SetActive(true);
            }
            else
            {
                natAnimator.SetTrigger("FadeIn");
            }
        } else if (character == "Cybergull")
        {
            if (Cybergull.activeSelf == false)
            {
                Cybergull.SetActive(true);
            }
            else
            {
                cybergullAnimator.SetTrigger("FadeIn");
            }
        }
        else if (character == "Dad")
        {
            if (Dad.activeSelf == false)
            {
                Dad.SetActive(true);
            }
            else
            {
                dadAnimator.SetTrigger("FadeIn");
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
        else if (character == "Pod")
        {
            podAnimator.SetTrigger("FadeOut");
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
        else if (character == "Nat")
        {
            natAnimator.SetTrigger("FadeOut");
        }
        else if (character == "Cybergull")
        {
            cybergullAnimator.SetTrigger("FadeOut");
        }
        else if (character == "Dad")
        {
            dadAnimator.SetTrigger("FadeOut");
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
        if (sfxName == "podOpen")
        {
            notificationSource.PlayOneShot(podOpen);
        }
        if (sfxName == "shortgull")
        {
            notificationSource.PlayOneShot(shortgull);
        }
        if (sfxName == "longgull")
        {
            notificationSource.PlayOneShot(longgull);
        }
        if (sfxName == "beep")
        {
            notificationSource.PlayOneShot(beep);
        }
        if (sfxName == "maxxMotorcycle")
        {
            notificationSource.PlayOneShot(maxxMotorcycle);
        }
        if (sfxName == "gullBattle")
        {
            notificationSource.PlayOneShot(gullBattle);
        }
        if (sfxName == "metalGrinding")
        {
            notificationSource.PlayOneShot(metalGrinding);
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
        if (audioName == "frisscity")
        {
            frisscitySource.Play();
        }
        if (audioName == "luxapt02")
        {
            luxapt02Source.Play();
        }
        if (audioName == "sundayneons")
        {
            sundayneonsSource.Play();
        }
        if (audioName == "deadroom")
        {
            deadroomSource.Play();
        }
        if (audioName == "ascensionmellow")
        {
            ascensionmellowSource.Play();
        }
        if (audioName == "loungelizards")
        {
            loungelizardsSource.Play();
        }
        if (audioName == "undergroundlab")
        {
            undergroundlabSource.Play();
        }
        if (audioName == "errorofourways")
        {
            errorofourwaysSource.Play();
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
        if (audioName == "frisscity")
        {
            frisscitySource.Stop();
        }
        if (audioName == "luxapt02")
        {
            luxapt02Source.Stop();
        }
        if (audioName == "sundayneons")
        {
            sundayneonsSource.Stop();
        }
        if (audioName == "deadroom")
        {
            deadroomSource.Stop();
        }
        if (audioName == "ascensionmellow")
        {
            ascensionmellowSource.Stop();
        }
        if (audioName == "loungelizards")
        {
            loungelizardsSource.Stop();
        }
        if (audioName == "undergroundlab")
        {
            undergroundlabSource.Stop();
        }
        if (audioName == "errorofourways")
        {
            errorofourwaysSource.Stop();
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
            lilguyCG.SetActive(false);
        }
        else if (backgroundName == "lilguyCG")
        {
            lilguyCG.SetActive(true);
            alleyway.SetActive(false);
        }
        else if (backgroundName == "cybergullsCG")
        {
            cybergullsCG.SetActive(true);
        }
    }
}
