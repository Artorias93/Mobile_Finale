using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class EndEndlessScreenController : EndGameScreenController
{
    [SerializeField] GameObject m_StarContainerToHide;
    [SerializeField] Text m_HighScoreText;
    [SerializeField] Text m_FinalScore;
    private void Awake()
    {
    }

    public void showEndGamePanel(int score, bool isHighScore)
    {
        base.showEndGamePanel(score);
        m_StarContainerToHide.SetActive(false);
        m_FinalScore.text = score.ToString();
        m_HighScoreText.gameObject.SetActive(isHighScore);
    }

    public override void showEndGamePanel(int score)
    {
        showEndGamePanel(score, false);
    }

}
