using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class EndGameScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] protected GameObject panel;
    [SerializeField] protected Image[] GO_stars;
    [SerializeField] private AMinigameManager m_currentManager;
    [SerializeField, Range(0.0f, 1.0f)] protected float i_hiddenAlpha = 0.2f;

    private void Awake()
    {      
        m_currentManager = GameObject.FindGameObjectWithTag(AMinigameManager.s_minigameTag).GetComponent<AMinigameManager>();
        Assert.AreEqual<int>(GO_stars.Length, 3, "Stars missing");

        for (int i = 0; i < GO_stars.Length; i++)
            hideStar(i);
    }

    public void showEndGamePanel(int score)
    {
        panel.SetActive(true);
        foreach(Transform child in panel.transform)
            child.gameObject.SetActive(true);
        for(int i = 0; i<score; i++)
        {
            showStar(i);
        }
    }

    protected void showStar(int index)
    {
        if (index >= 0 && index < GO_stars.Length)
            GO_stars[index].color = Color.white;
        else
            Debug.Log("Invalid Index");
    }

    protected void hideStar(int index)
    {
        if (index >= 0 && index < GO_stars.Length)
            GO_stars[index].color = new Color(1f, 1f, 1f, i_hiddenAlpha);
        else
            Debug.Log("Invalid Index");
    }

    public void hideEverything()
    {
        panel.SetActive(false);
    }

    public void RequestClosing()
    {
        m_currentManager?.SetReadyToClose();
    }

    public void RequestRetry()
    {
        GameManager.m_instance.RetryCurrentScene();
    }
}

