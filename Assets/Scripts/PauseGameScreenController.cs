using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject blockPanel;

    private AMinigameManager m_MinigameManager;

    private void Awake()
    {
        m_MinigameManager = GameObject.FindGameObjectWithTag(AMinigameManager.s_minigameTag).GetComponent<AMinigameManager>();
        if(m_MinigameManager == null)
        {
            Debug.LogWarning("Couldn't find a minigame manager for pause controller");
        }
    }

    public void RequestPause()
    {
        if(m_MinigameManager != null)
            m_MinigameManager.PauseGame();
    }

    public void RequestResume()
    {
        if(m_MinigameManager!=null)
            m_MinigameManager.ResumeGame();
    }

    public void showPausePanel() {
        panel.SetActive(true);
        blockPanel.SetActive(true);
    }

    public void hidePausePanel() {
        panel.SetActive(false);
        blockPanel.SetActive(false);
    }

    public void RequestMainMenu()
    {
        GameManager.m_instance.ReturnToMainMenu();
    }
}
