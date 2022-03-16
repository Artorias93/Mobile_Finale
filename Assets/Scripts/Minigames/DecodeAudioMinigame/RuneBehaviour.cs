using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class RuneBehaviour : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private Image m_runeImage;
    [SerializeField] private Text m_nameTxt;
    
    [Header("References")]
    [SerializeField] private AudioSource m_audioSource;

    private RuneEntry m_myEntry;

    private void Awake()
    {
        Assert.IsNotNull(m_runeImage, "No Rune Img Reference Selected");
        Assert.IsNotNull(m_nameTxt, "No Text Reference Selected");
    }

    public void Initialize(RuneEntry runeEntry)
    {
        m_myEntry = runeEntry;

        m_runeImage.sprite = runeEntry.m_sprite;
        m_nameTxt.text = runeEntry.m_runeText;

        m_audioSource.clip = runeEntry.m_audioClip;
    }

    public void selectedRune()
    {
        DecodeAudioManager.m_instance.checkEntry(m_myEntry);
    }
}
