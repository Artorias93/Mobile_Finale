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

    private int i_savedScore = 0;

    private void Awake()
    {
        if (m_btn == null)
            m_btn = GetComponent<Button>();
        Assert.IsNotNull(m_btn);

        m_btn.onClick.AddListener(buttonCallback);

        //Load score if present
        if (PlayerPrefs.HasKey(m_recipe.name))
            i_savedScore = PlayerPrefs.GetInt(m_recipe.name);
    }

    private void buttonCallback()
    {
        GameManager.m_instance.StartRecipe(m_recipe);
    }
}
