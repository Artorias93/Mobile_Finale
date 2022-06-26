using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessModeButton : MonoBehaviour
{
    [SerializeField] private Button m_btn;
    [SerializeField] private Text m_HighScoreText;
    private int i_HighScore = 0;
    void Awake()
    {
        m_btn = GetComponent<Button>();
        if (m_btn != null)
            m_btn.onClick.AddListener(ButtonCallback);
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(GameManager.EndlessSaveString))
        {
            i_HighScore = PlayerPrefs.GetInt(GameManager.EndlessSaveString);
            m_HighScoreText.text = "High Score: " + i_HighScore.ToString();
        }
    }

    private void ButtonCallback()
    {
        GameManager.m_instance.SetReturnPage(PageToReturn.Main);
        GameManager.m_instance.StartEndlessMode();
    }
}
