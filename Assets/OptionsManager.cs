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
    private void Start()
    {
        Assert.IsNotNull(m_mainMenuSlider);
        Assert.IsNotNull(m_minigameSlider);

        //Loads current settings from Music Manager
        m_MusicManager = MusicManager.Instance;

        m_mainMenuSlider.value = m_MusicManager.GetMainMenuMusicVolume();
        m_minigameSlider.value = m_MusicManager.GetMinigameMusicVolume();
    }
}
