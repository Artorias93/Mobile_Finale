using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class EndEndlessScreenController : EndGameScreenController
{
    [SerializeField] Text m_HighScoreText;
    private void Awake()
    {
        Assert.AreEqual<int>(GO_stars.Length, 3, "Stars missing");

        for (int i = 0; i < GO_stars.Length; i++)
            hideStar(i);
    }

    public void showEndGamePanel(int score, bool isHighScore)
    {
        base.showEndGamePanel(score);
        m_HighScoreText.gameObject.SetActive(isHighScore);
    }

    public override void showEndGamePanel(int score)
    {
        showEndGamePanel(score, false);
    }

}
