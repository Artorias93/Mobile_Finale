using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class RecipeLoadButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_btn;
    [SerializeField] private Image m_Star;

    [Header("Recipe Options")]
    [SerializeField] private SORecipe m_recipe;

    [Header("Score options")]
    [SerializeField] private Color c_color0;
    [SerializeField] private Color c_color1;
    [SerializeField] private Color c_color2;
    [SerializeField] private Color c_color3;

    private int i_savedScore = 0;

    private void Awake()
    {
        if (m_btn == null)
            m_btn = GetComponent<Button>();
        Assert.IsNotNull(m_btn);
        Assert.IsNotNull(m_Star);

        m_btn.onClick.AddListener(buttonCallback);

        //Load score if present
        if (PlayerPrefs.HasKey(m_recipe.name))
            i_savedScore = PlayerPrefs.GetInt(m_recipe.name);
        switch (i_savedScore)
        {
            case 0:
                m_Star.color = c_color0;
                break;
            case 1:
                m_Star.color = c_color1;
                break;
            case 2:
                m_Star.color = c_color2;
                break;
            case 3:
                m_Star.color = c_color3;
                break;
            default:
                m_Star.color = c_color0;
                break;
        }
    }

    private void buttonCallback()
    {
        GameManager.m_instance.StartRecipe(m_recipe);
    }
}
