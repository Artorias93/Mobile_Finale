using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class SceneLoadButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_btn;

    [Header("Next Scene Options")]
    [SerializeField] private string m_minigameScene;
    [SerializeField] private Difficulty m_difficulty = Difficulty.EASY;
    [SerializeField] private difficultyUI m_dUI;


    private void Awake()
    {
        if(m_btn == null)
            m_btn = GetComponent<Button>();
        Assert.IsNotNull(m_btn);

        m_btn.onClick.AddListener(buttonCallback);
    }

    public void cycleSelectedDifficulty()
    {
        int currentDifficultyValue = (int)m_difficulty;
        int nextDifficultyValue = (currentDifficultyValue + 1) % 3; //Cycle through easy medium hard, starting over

        m_difficulty = (Difficulty) nextDifficultyValue;
        m_dUI.UpdateShownDifficulty(m_difficulty);
    }

    private void buttonCallback()
    {
        GameManager.m_instance.StartLoadingCoroutine(m_minigameScene, m_difficulty);
    }
}
