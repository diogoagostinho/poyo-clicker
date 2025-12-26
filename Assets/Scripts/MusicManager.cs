using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    [Header("Fade Settings")]
    public float maxVolume = 0.5f;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
    public float loopGap = 0.1f; // tiny pause between loops

    void Start()
    {
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        while (true)
        {
            // Fade in
            musicSource.volume = 0f;
            musicSource.Play();
            yield return StartCoroutine(FadeVolume(0f, maxVolume, fadeInDuration));

            // Wait until near the end
            yield return new WaitForSeconds(musicSource.clip.length - fadeOutDuration);

            // Fade out
            yield return StartCoroutine(FadeVolume(maxVolume, 0f, fadeOutDuration));
            musicSource.Stop();

            yield return new WaitForSeconds(loopGap);
        }
    }

    IEnumerator FadeVolume(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        musicSource.volume = to;
    }
}
