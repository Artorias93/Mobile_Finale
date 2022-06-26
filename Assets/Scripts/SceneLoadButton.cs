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
    [SerializeField] private DifficultyButton m_difficultyButton;
    [SerializeField] private PageToReturn m_OwnerPage = PageToReturn.Training1;

    private void Awake()
    {
        if(m_btn == null)
            m_btn = GetComponent<Button>();
        Assert.IsNotNull(m_btn);

        m_btn.onClick.AddListener(buttonCallback);
    }

    private void buttonCallback()
    {
        GameManager.m_instance.SetReturnPage(m_OwnerPage);
        GameManager.m_instance.StartLoadingCoroutine(m_minigameScene, m_difficultyButton.getSelectedDifficulty());
    }
}
