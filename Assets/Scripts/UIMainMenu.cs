using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum PageToReturn
{
    Main = 0, Recipe1 = -1,Recipe2 = -2, Recipe3 = -3 , Training1 = 1, Training2 = 2
}

public class UIMainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_EndRecipe;
    [SerializeField] private GameObject m_RecipeScreen;
    [SerializeField] private GameObject[] m_RecipePages;
    [SerializeField] private GameObject m_TrainingScreen;
    [SerializeField] private GameObject[] m_TrainingPages;
    [SerializeField] private EndRecipeScreenController m_EndRecipeScreenController;

    public static string Tag = "MainMenu";

    public void ShowEndRecipe(int finalScore)
    {
        m_MainMenu.SetActive(false); //The scene gets loaded with The main menu active, so we must deactivate it

        m_EndRecipe.SetActive(true);
        m_EndRecipeScreenController.showEndGamePanel(finalScore);
    }

    public void ReturnToPage(PageToReturn page)
    {
        //Switches based on page, activating the correct set of gameobjects. Assumes that the scene has been freshly opened
        switch (page)
        {
            case PageToReturn.Main:
                OpenMainMenu();
                break;
            case PageToReturn.Recipe1:
                OpenRecipes(0);
                break;
            case PageToReturn.Recipe2:
                OpenRecipes(1);
                break;
            case PageToReturn.Recipe3:
                OpenRecipes(2);
                break;

            case PageToReturn.Training1:
                OpenTraining(0);
                break;
            case PageToReturn.Training2:
                OpenTraining(1);
                break;
            default:
                break;

        }
    }

    private void OpenRecipes(int page)
    {
        m_MainMenu.SetActive(false);
        m_RecipeScreen.SetActive(true);
        foreach (GameObject PageGO in m_RecipePages)
            PageGO.SetActive(false);
        if (page >= 0 && page < m_RecipePages.Length)
            m_RecipePages[page].SetActive(true);
    }
    private void OpenTraining(int page)
    {
        m_TrainingScreen.SetActive(true);
        m_MainMenu.SetActive(false);
        foreach (GameObject PageGO in m_TrainingPages)
            PageGO.SetActive(false);
        if (page >= 0 && page < m_TrainingPages.Length)
            m_TrainingPages[page].SetActive(true);

    }

    private void OpenMainMenu()
    {
        m_MainMenu.SetActive(true);
    }
}
