using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_EndRecipe;
    [SerializeField] private EndRecipeScreenController m_EndRecipeScreenController;

    public static string Tag = "MainMenu";

    public void ShowEndRecipe(int finalScore)
    {
        m_MainMenu.SetActive(false); //The scene gets loaded with The main menu active, so we must deactivate it

        m_EndRecipe.SetActive(true);
        m_EndRecipeScreenController.showEndGamePanel(finalScore);
    }
}
