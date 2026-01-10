using UnityEngine;

public class ClickSoundManager : MonoBehaviour
{
    public static ClickSoundManager Instance;

    [Header("Audio Source")]
    public AudioSource clickSource;

    [Header("Sounds")]
    public AudioClip defaultClickSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetClickSound(AudioClip clip)
    {
        clickSource.clip = clip;
    }

    public void ResetToDefault()
    {
        clickSource.clip = defaultClickSound;
    }

    public void PlayClick()
    {
        clickSource.Play();
    }
}
