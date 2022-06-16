using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class RecipeLoadButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_btn;
    [SerializeField] private GameObject[] m_Stars;

    [Header("Recipe Options")]
    [SerializeField] private SORecipe m_recipe;

    [Header("Score options")]
    [SerializeField] private Color c_color0;
    [SerializeField] private Color c_color1;
    [SerializeField] private Color c_color2;
    [SerializeField] private Color c_color3;

    private int i_savedScore = 0;
    private Image[] m_StarImages;

    private void Awake()
    {
        if (m_btn == null)
            m_btn = GetComponent<Button>();
        Assert.IsNotNull(m_btn);
        Assert.IsNotNull(m_Stars);

        m_StarImages = new Image[m_Stars.Length];
        for (int i = 0; i < m_Stars.Length; i++)
        {
            m_Stars[i].SetActive(false);
            m_StarImages[i] = m_Stars[i].GetComponent<Image>();
        }

        m_btn.onClick.AddListener(buttonCallback);
    }

    private void OnEnable()
    {
        //Load score if present, done on enable to manage when changes happen in the main menu (for example player deleting saves)
        if (PlayerPrefs.HasKey(m_recipe.name))
            i_savedScore = PlayerPrefs.GetInt(m_recipe.name);
        else
            i_savedScore = 0;

        ShowStars(i_savedScore);
    }

    private void buttonCallback()
    {
        GameManager.m_instance.SetReturnPage(PageToReturn.Recipe);
        GameManager.m_instance.StartRecipe(m_recipe);
    }

    private void ShowStars(int num)
    {
        if(num >= 1 && num <= 3)
        {
            foreach(GameObject star in m_Stars)
            {
                star.SetActive(true);
            }

            Color colorToPaint = c_color0;
            switch (num)
            {
                case 1:
                    colorToPaint = c_color1;
                    break;
                case 2:
                    colorToPaint = c_color2;
                    break;
                case 3:
                    colorToPaint = c_color3;
                    break;
            }

            for(int i = 0; i< num; i++)
            {
                m_StarImages[i].color = colorToPaint;
            }
            for(int i = num; i<m_StarImages.Length; i++)
            {
                m_StarImages[i].color = c_color0;
            }
        }
    }
}
