using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class difficultyUI : MonoBehaviour
{
    [SerializeField] private Text m_BtnText;

    public Difficulty SelectedDifficulty
    {
        get { return difficulty; }
    }

    private Difficulty difficulty=Difficulty.EASY;

    private void Awake()
    {
        if(m_BtnText == null)   
            m_BtnText = GetComponentInChildren<Text>();
    }

    //Could be updated in the future to control a sprite instead
    public void UpdateShownDifficulty(Difficulty new_difficulty)
    {
        if (new_difficulty == Difficulty.EASY) m_BtnText.text = "Easy";
        else if (new_difficulty == Difficulty.MEDIUM) m_BtnText.text = "Medium";
        else if (new_difficulty == Difficulty.HARD) m_BtnText.text = "Hard";
        difficulty = new_difficulty;
    }
}
