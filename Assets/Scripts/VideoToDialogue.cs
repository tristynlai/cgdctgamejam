using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class VideoToDialogue : MonoBehaviour, IDataPersistence
{
    [Header("Video and Dialogue")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private DialogueRunner dialogueRunner;
    
    [Header("Environment")]
    [SerializeField] private GameObject gameEnvironment; 

    [Header("Fade Function")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeOutDuration = 1f; 
    [SerializeField] private float fadeInDuration = 2.5f; 

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI; 

    [Header("Dialogue Node")]
    [SerializeField] private string startingNode = "Intro";

    private bool hasSeenCutscene = false;

    public void LoadData(GameData data) {
        hasSeenCutscene = data.HasSeenIntroCutscene;

        if (hasSeenCutscene) {
            StopAllCoroutines();
            SkipToEnvironment();
        }
    }

    public void SaveData(ref GameData data) {
        data.HasSeenIntroCutscene = hasSeenCutscene;
    }

    private void Start()
    {
        StartCoroutine(StartAfterLoadCheck());
    }

    private IEnumerator StartAfterLoadCheck()
{
        yield return null;

        if (hasSeenCutscene) {
            yield break;
        }

        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 1f; 
        }

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            StartCoroutine(WaitForVideoToStart());
        }
    }

    private void SkipToEnvironment() {
        if (videoPlayer != null) {
            videoPlayer.gameObject.SetActive(false);
        }

        if (gameEnvironment != null) {
            gameEnvironment.SetActive(true);
        }

        if (fadeOverlay != null) {
            fadeOverlay.alpha = 0f;
        }
    }

    private IEnumerator WaitForVideoToStart()
    {
        while (videoPlayer != null && !videoPlayer.isPlaying)
        {
            yield return null;
        }

        if (fadeOverlay != null)
        {
            StartCoroutine(Fade(1f, 0f, fadeOutDuration)); 
        }
    }

    private void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        hasSeenCutscene = true;

        if (DataPersistenceManager.instance != null) {
            DataPersistenceManager.instance.SaveGame();
        }

        StartCoroutine(ExecuteStoryboardSequence());
    }

    private IEnumerator ExecuteStoryboardSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration));

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false); 
        }
        
        if (gameEnvironment != null)
        {
            gameEnvironment.SetActive(true); 
        }

        yield return StartCoroutine(Fade(1f, 0f, fadeInDuration));

    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        if (fadeOverlay != null)
        {
            float time = 0;
            fadeOverlay.alpha = startAlpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                yield return null;
            }
            
            fadeOverlay.alpha = targetAlpha;
        }
    }
}
