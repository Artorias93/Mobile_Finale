using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// This class is the overall game manager, tasked with managing scene changes, recipes loading, score storing, and general overall flow
    /// </summary>

    public static GameManager m_instance;
    private AMinigameManager m_minigameManager;

    private List<int> i_scores;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    public void StartLoadingCoroutine(string minigameName, Difficulty difficulty)
    {
        StartCoroutine(_loadMinigame(minigameName, difficulty));
    }

    private IEnumerator _loadMinigame(string minigameName, Difficulty difficulty)
    {

        AsyncOperation loading = SceneManager.LoadSceneAsync(minigameName, LoadSceneMode.Single);
        yield return new WaitUntil(() => { return loading.isDone; } );

        m_minigameManager = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<AMinigameManager>();
        m_minigameManager.BeginMinigame(difficulty);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(minigameName));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(_asyncSceneLoad("Mainmenu"));
    }

    private IEnumerator _asyncSceneLoad(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return new WaitUntil(() => { return loading.isDone; });
    }

    public void StartRecipe(SORecipe recipe)
    {
        StartCoroutine(PlayRecipe(recipe));
    }
    private IEnumerator PlayRecipe(SORecipe recipe)
    {
        if (recipe == null)
        {
            Debug.LogWarning("Passed a null recipe");
            yield return null;
        }
        i_scores = new List<int>();

        for (int i = 0; i<recipe.getLength(); i++)
        {
            RecipeEntry nextLevel = recipe.getEntry(i);

            Debug.Log("Loading scene: " + nextLevel.m_minigameScene.name);
            yield return StartCoroutine(_loadMinigame(nextLevel.m_minigameScene.name, nextLevel.m_difficulty));
            Debug.Log("Scene loaded");
            if(m_minigameManager == null)
            {
                Debug.Log("Can't find Minigame Manager");
                yield return null;
            }

            yield return new WaitUntil(() => { return m_minigameManager.CanCloseGame; });
            i_scores.Add(m_minigameManager.Score);
            Debug.Log("Closing Game and proceding to next");
        }

        Debug.Log("Played all scenes");
        //Load end recipe scene
        ReturnToMainMenu();
    }
}
