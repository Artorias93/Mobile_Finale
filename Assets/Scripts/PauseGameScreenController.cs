using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private Text retryTxt;
    [SerializeField] private Text returnTxt;

    private AMinigameManager m_MinigameManager;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag(AMinigameManager.s_minigameTag);
        m_MinigameManager = go.GetComponent<AMinigameManager>();
        if(m_MinigameManager == null)
        {
            Debug.LogWarning("Couldn't find a minigame manager for pause controller");
        }

        if (GameManager.m_instance.CurrRecipe == null)
        {
            if (GameManager.m_instance.IsEndless)
            {
                retryTxt.text = "Restart Endless";
                returnTxt.text = "Return To Menu";
            }
            else
            {
                retryTxt.text = "Retry Level";
                returnTxt.text = "Return To Selection";
            }
        }
        else
        {
            retryTxt.text = "Retry Recipe";
            returnTxt.text = "Return To Menu";
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

    public void RequestRetryScene()
    {
        if (GameManager.m_instance.CurrRecipe == null)
        {
            if (GameManager.m_instance.IsEndless) RequestRetryEndless();
            else GameManager.m_instance.RetryCurrentScene();
        }
        else RequestRetryRecipe();
    }

    public void RequestRetryRecipe()
    {
        GameManager.m_instance.RetryCurrentRecipe();
    }
    public void RequestRetryEndless()
    {
        GameManager.m_instance.RetryEndless();
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
