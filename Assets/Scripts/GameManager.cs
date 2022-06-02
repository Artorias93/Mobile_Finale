using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// This class is the overall game manager, tasked with managing scene changes, recipes loading, score storing, and general overall flow
    /// </summary>


    [SerializeField] private MusicManager m_musicManager;
    [SerializeField] private TransitionManager m_TransitionManager;
    [SerializeField] private float f_waitTimeBetweenScenes = 1f;

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

        Assert.IsNotNull(m_musicManager);
        Assert.IsNotNull(m_TransitionManager);

        DontDestroyOnLoad(gameObject);
    }
    public void StartLoadingCoroutine(string minigameName, Difficulty difficulty)
    {
        //Used for single minigames
        StartCoroutine(_loadMinigame(minigameName, difficulty));
    }

    private IEnumerator _loadMinigame(string minigameName, Difficulty difficulty)
    {
        //used for single minigames NOT recipes
        m_minigameManager = null;


        
        m_musicManager.switchToMinigame();

        yield return StartCoroutine(_asyncSceneLoad(minigameName));

        m_minigameManager = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<AMinigameManager>();
        m_minigameManager.BeginMinigame(difficulty);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(minigameName));

        yield return new WaitUntil(() => { return m_minigameManager.CanCloseGame; } );
        ReturnToMainMenu();
    }

    private IEnumerator _loadEntry(RecipeEntry entry)
    {
        m_minigameManager = null;

        
        Debug.Log("Loading scene: " + entry.m_minigameScene.name);

        m_musicManager.switchToMinigame();
        yield return StartCoroutine(_asyncSceneLoad(entry.m_minigameScene.name));

        m_minigameManager = GameObject.FindGameObjectWithTag(AMinigameManager.s_minigameTag).GetComponent<AMinigameManager>();
        if (m_minigameManager == null)
        {
            Debug.Log("Can't find Minigame Manager, returning to main menu");
            ReturnToMainMenu();
        }
        else
        {
            m_minigameManager.BeginMinigame(entry.m_difficulty);
            Debug.Log("Scene loaded");
        }
        
        
    }

    public void ReturnToMainMenu()
    {
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
        }
        m_musicManager.switchToMainMenu();
        StartCoroutine(_asyncSceneLoad("Mainmenu"));
    }

    private IEnumerator _asyncSceneLoad(string sceneName)
    {
        m_TransitionManager.StartLoadingTransition();
        

        yield return new WaitUntil(() => m_TransitionManager.AnimationCompleted);
        Debug.Log("Transition Complete, start loading");

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return new WaitUntil(() => loading.isDone);
        Debug.Log("Loading Complete, start detransitioning");

        m_TransitionManager.StartReadyTransition();
        yield return new WaitUntil(() => m_TransitionManager.AnimationCompleted);
        Debug.Log("Detransition complete");
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

            yield return StartCoroutine(_loadEntry(nextLevel));

            yield return new WaitUntil(() => { return m_minigameManager.CanCloseGame; });
            i_scores.Add(m_minigameManager.Score);

            Debug.Log("Closing Game and proceding to next");
            
        }

        Debug.Log("Played all scenes");
        //Load end recipe scene
        ReturnToMainMenu();
    }
}
