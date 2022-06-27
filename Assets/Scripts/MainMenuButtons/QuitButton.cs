using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuitButton : MonoBehaviour
{
    [SerializeField] private Button m_btn;
    void Awake()
    {
        m_btn = GetComponent<Button>();
        if (m_btn != null)
            m_btn.onClick.AddListener(ButtonCallback);
    }

    private void ButtonCallback()
    {
        Application.Quit();
    }
}
