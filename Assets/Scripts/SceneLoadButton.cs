using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class SceneLoadButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button btn;

    [Header("Next Scene Options")]
    [SerializeField] private string m_sceneName;
    [SerializeField] private Difficulty m_difficulty;

    private void Awake()
    {
        if(btn == null)
            btn = GetComponent<Button>();
        Assert.IsNotNull(btn);

        btn.onClick.AddListener(buttonCallback);
    }

    private void buttonCallback()
    {
        GameManager.m_instance.StartLoadingCoroutine(m_sceneName, m_difficulty);
    }
}
