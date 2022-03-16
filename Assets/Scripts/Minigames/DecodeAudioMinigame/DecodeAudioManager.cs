using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct RuneEntry{
    public string m_runeText;
    public Sprite m_sprite;
    public AudioClip m_audioClip;
}

public class DecodeAudioManager : MonoBehaviour
{
    [Header("Game Parameters")]
    [SerializeField] private RuneEntry[] m_runeDictionary;
    [SerializeField] private int m_runeCount = 4;

    [Header("References")]
    [SerializeField] private BookManager m_bookManager;
    [SerializeField] private AudioSource m_audioSource;


    private List<RuneEntry> m_selectedRunes;
    private int m_correctIndex;

    private void Awake()
    {
        Assert.IsNotNull(m_bookManager);
        Assert.IsFalse(m_runeDictionary.Length < m_runeCount, "Impossible to populate screen, to little runes");

        m_selectedRunes = new List<RuneEntry>();
        while(m_selectedRunes.Count < m_runeCount)
        {
            int index = Random.Range(0, m_runeCount);
            RuneEntry entry = m_runeDictionary[index];
            if (!m_selectedRunes.Contains(entry))
            {
                m_selectedRunes.Add(entry);
            }
        }

        m_correctIndex = Random.Range(0, m_selectedRunes.Count);
    }

    private void Start()
    {
        m_bookManager.Initialize(m_selectedRunes);
        m_audioSource.PlayOneShot(m_selectedRunes[m_correctIndex].m_audioClip);
    }
}
