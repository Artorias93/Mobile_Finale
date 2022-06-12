using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class MusicManager : MonoBehaviour
{
    [Header("Songs")]
    [SerializeField] private AudioClip[] m_mainMenu;
    [SerializeField, Range(0, 1)] private float f_mainMenuVolume = 0.6f;
    [SerializeField] private AudioClip[] m_minigameMusic;
    [SerializeField, Range(0, 1)] private float f_minigameMusicVolume = 0.5f;

    [Header("Settings")]
    [SerializeField] private float f_FadeOutSpeed = 0.1f;
    [SerializeField] private float f_FadeInSpeed = 0.1f;

    [Header("Reference")]
    [SerializeField] private AudioSource m_musicSource;

    private bool b_Fading = false;
    private bool b_Switching = false;

    public bool Fading { get { return b_Fading; } }
    public bool Switching { get { return b_Switching; } }

    public static MusicManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }

        if(m_musicSource == null)
            m_musicSource = GetComponent<AudioSource>();
        Assert.IsNotNull(m_musicSource);

        //Load saved volumes
        if (PlayerPrefs.HasKey(OptionsManager.MainMenuMusicSaveString))
            f_mainMenuVolume = PlayerPrefs.GetFloat(OptionsManager.MainMenuMusicSaveString);
        if (PlayerPrefs.HasKey(OptionsManager.MinigameMusicSaveString))
            f_minigameMusicVolume = PlayerPrefs.GetFloat(OptionsManager.MinigameMusicSaveString);

        m_musicSource.volume = f_mainMenuVolume;

        m_musicSource.clip = GetRandomClip(ref m_mainMenu);
        m_musicSource.Play();


    }

    public void switchToMainMenu()
    {
        AudioClip clip = GetRandomClip(ref m_mainMenu);
        StartCoroutine(FadeInAndOut(f_mainMenuVolume, clip));
    }

    public void switchToMinigame()
    {
        AudioClip clip = GetRandomClip(ref m_minigameMusic);
        StartCoroutine(FadeInAndOut(f_minigameMusicVolume, clip));
    }

    public IEnumerator FadeInAndOut(float target, AudioClip ClipToPlay)
    {
        b_Fading = true;
        b_Switching = false;
        yield return StartCoroutine(FadeOut());
        b_Switching = true;
        yield return StartCoroutine(FadeIn(target, ClipToPlay));
        b_Fading = false;
        yield return null;
    }

    public IEnumerator FadeOut()
    {
        while(m_musicSource.volume > 0)
        {
            m_musicSource.volume -= f_FadeOutSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        yield return null;
    }
    
    public IEnumerator FadeIn(float target, AudioClip ClipToPlay)
    {
        m_musicSource.clip = ClipToPlay;
        m_musicSource.Play();
        while (m_musicSource.volume < target)
        {
            m_musicSource.volume += f_FadeInSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        yield return null;
    }

    private AudioClip GetRandomClip(ref AudioClip[] audioClips)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        return audioClips[randomIndex];
    }

    public void SetMainMenuVolume(float volume)
    {
        if (volume >= 0 && volume <= 1)
        {
            m_musicSource.volume = volume;
            f_mainMenuVolume = volume;
        }
    }
    public void SetMinigameMusicVolume(float volume)
    {
        if (volume >= 0 && volume <= 1)
        {
            f_minigameMusicVolume = volume;
        }
    }

    public float GetMainMenuMusicVolume()
    {
        return f_mainMenuVolume;
    }
    public float GetMinigameMusicVolume()
    {
        return f_minigameMusicVolume;
    }
}
