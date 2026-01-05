using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class SecretBossVideoPlayer : MonoBehaviour
{
    public static SecretBossVideoPlayer Instance;

    public CanvasGroup canvasGroup;
    public VideoPlayer videoPlayer;
    public AudioSource videoAudio;

    float previousTimeScale;
    AudioSource[] allAudioSources;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlaySBVideo()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        // PAUSE GAME
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // STOP ALL OTHER AUDIO
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var a in allAudioSources)
        {
            if (a != videoAudio)
                a.Pause();
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        videoPlayer.Play();
        videoAudio.Play();

        yield return null;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        GogetaBossManager.Instance.StartGogetaFight();
        videoPlayer.Stop();
        videoAudio.Stop();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        // RESUME GAME
        Time.timeScale = previousTimeScale;

        foreach (var a in allAudioSources)
        {
            if (a != videoAudio)
                a.UnPause();
        }

        yield return null;

        
    }
}
