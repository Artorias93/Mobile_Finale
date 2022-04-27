using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class difficultyUI : MonoBehaviour
{
    [SerializeField] private Text m_BtnText;

    private void Awake()
    {
        if(m_BtnText == null)   
            m_BtnText = GetComponentInChildren<Text>();
    }

    //Could be updated in the future to control a sprite instead
    public void UpdateShownDifficulty(Difficulty difficulty)
    {
        if (difficulty == Difficulty.EASY) m_BtnText.text = "Easy";
        else if (difficulty == Difficulty.MEDIUM) m_BtnText.text = "Medium";
        else if (difficulty == Difficulty.HARD) m_BtnText.text = "Hard";
    }
}
