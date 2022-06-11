using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
public class OptionsManager : MonoBehaviour
{
    [Header("References")]
    private MusicManager m_MusicManager; //Loaded via Singleton to avoid problems with don't destroy on load
    [SerializeField] private Slider m_mainMenuSlider;
    [SerializeField] private Slider m_minigameSlider;
    [SerializeField] private string m_MainMenuMusicSaveString = "MainMenuVolume";
    [SerializeField] private string m_MinigameMusicSaveString = "MinigameVolume";

    private void Start()
    {
        Assert.IsNotNull(m_mainMenuSlider);
        Assert.IsNotNull(m_minigameSlider);

        //Loads current settings from Music Manager
        m_MusicManager = MusicManager.Instance;
        if (PlayerPrefs.HasKey(m_MainMenuMusicSaveString))
        {
            float volume = PlayerPrefs.GetFloat(m_MainMenuMusicSaveString);
            m_MusicManager.SetMainMenuVolume(volume);
            m_mainMenuSlider.value = volume;
        } else
        {
            m_mainMenuSlider.value = m_MusicManager.GetMainMenuMusicVolume();
        }
        if (PlayerPrefs.HasKey(m_MinigameMusicSaveString))
        {
            float volume = PlayerPrefs.GetFloat(m_MinigameMusicSaveString);
            m_MusicManager.SetMinigameMusicVolume(volume);
            m_minigameSlider.value = volume;
        }
        else
        {
            m_minigameSlider.value = m_MusicManager.GetMinigameMusicVolume();
        }

    }

    public void MainMenuMusicSliderChanged()
    {
        m_MusicManager.SetMainMenuVolume(m_mainMenuSlider.value);
        PlayerPrefs.SetFloat(m_MainMenuMusicSaveString, m_mainMenuSlider.value);
    }
    public void MinigameMusicSliderChanged()
    {
        m_MusicManager.SetMinigameMusicVolume(m_minigameSlider.value);
        PlayerPrefs.SetFloat(m_MinigameMusicSaveString, m_minigameSlider.value);

    }

    public void DeleteAllSaves()
    {
        PlayerPrefs.DeleteAll();
        MainMenuMusicSliderChanged();
        MinigameMusicSliderChanged();
    }
}
