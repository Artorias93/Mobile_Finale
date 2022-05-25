using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class RecipeLoadButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_btn;

    [Header("Recipe Options")]
    [SerializeField] private SORecipe m_recipe;

    private void Awake()
    {
        if (m_btn == null)
            m_btn = GetComponent<Button>();
        Assert.IsNotNull(m_btn);

        m_btn.onClick.AddListener(buttonCallback);
    }

    private void buttonCallback()
    {
        GameManager.m_instance.StartRecipe(m_recipe);
    }
}
