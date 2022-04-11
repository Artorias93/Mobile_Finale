using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject winContent;
    [SerializeField] private GameObject loseContent;

    public void showEndGamePanel(bool wonGame)
    {
        if (wonGame)
            showWinningScreen();
        else
            showLosingScreen();
    }

    public void showWinningScreen()
    {
        loseContent.SetActive(false);
        panel.SetActive(true);
        winContent.SetActive(true);
    }

    public void hideEverything()
    {
        panel.SetActive(false);
        loseContent.SetActive(false);
        winContent.SetActive(false);
    }

    public void showLosingScreen()
    {
        panel.SetActive(true);
        winContent.SetActive(false);
        loseContent.SetActive(true);
    }
}

