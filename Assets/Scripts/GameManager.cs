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

    [Header("Endless mode")]
    [SerializeField] List<string> m_ScenesName;

    public static GameManager m_instance;
    private AMinigameManager m_minigameManager;

    private List<int> i_scores;
    private Difficulty diff;
    private SORecipe currentRecipe;
    private bool b_endless = false;

    private PageToReturn m_SavedPage = PageToReturn.Main;

    public SORecipe CurrRecipe
    {
        get { return currentRecipe; }
    }
    public bool IsEndless
    {
        get { return b_endless; }
    }

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
        Assert.IsNotNull(m_ScenesName);
        Assert.AreNotEqual<int>(0, m_ScenesName.Count, "List of scenes is empty");
        DontDestroyOnLoad(gameObject);
    }
    public void StartLoadingCoroutine(string minigameName, Difficulty difficulty)
    {
        //Used for single minigames
        diff = difficulty;
        StartCoroutine(_loadMinigame(minigameName, difficulty));
    }

    private IEnumerator _loadMinigame(string minigameName, Difficulty difficulty)
    {
        //used for single minigames NOT recipes
        diff = difficulty;
        m_minigameManager = null;


        
        m_musicManager.switchToMinigame();

        yield return StartCoroutine(_asyncSceneLoad(minigameName));

        m_minigameManager = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<AMinigameManager>();
        m_minigameManager.BeginMinigame(difficulty);

        yield return new WaitUntil(() => {
            if (m_minigameManager)
                return m_minigameManager.CanCloseGame;
            else
                return false;
        });
        if(m_minigameManager != null)
            ReturnToMainMenu();
    }

    private IEnumerator _loadEntry(RecipeEntry entry)
    {
        m_minigameManager = null;

        
        Debug.Log("Loading scene: " + entry.m_minigameScene);

        m_musicManager.switchToMinigame();
        yield return StartCoroutine(_asyncSceneLoad(entry.m_minigameScene));

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

    public void RetryCurrentScene()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        StopAllCoroutines();
        m_musicManager.switchToMinigame();
        StartLoadingCoroutine(SceneManager.GetActiveScene().name, diff);
    }

    public void RetryCurrentRecipe()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        StopAllCoroutines();
        m_musicManager.switchToMinigame();
        StartRecipe(currentRecipe);
    }
    public void RetryEndless()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        StopAllCoroutines();
        m_musicManager.switchToMinigame();
        StartEndlessMode();
    }

    public void ReturnToMainMenu()
    {
        //Used to return to main menu stopping minigame midway through (es. pause, return to main menu)
        if (Time.timeScale == 0) 
        {
            Time.timeScale = 1;
        }

        b_endless = false;
        currentRecipe = null;
        StopAllCoroutines();
        m_musicManager.switchToMainMenu();
        StartCoroutine(_asyncSceneLoadMainMenuStopping());
    }

    public void ReturnToMainMenuNonStopping()
    {
        //Variant of return to main menu which doesn't stop current coroutine
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        b_endless = false;
        currentRecipe = null;
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
        //yield return new WaitUntil(() => m_TransitionManager.AnimationCompleted);
        Debug.Log("Detransition complete");
    }    
    private IEnumerator _asyncSceneLoadMainMenuStopping()
    {
        m_TransitionManager.StartLoadingTransition();
        

        yield return new WaitUntil(() => m_TransitionManager.AnimationCompleted);
        Debug.Log("Transition Complete, start loading");

        AsyncOperation loading = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        yield return new WaitUntil(() => loading.isDone);
        Debug.Log("Loading Complete, start detransitioning");

        m_TransitionManager.StartReadyTransition();
        Debug.Log("Detransition complete");

        UIMainMenu MainMenu = GameObject.FindGameObjectWithTag(UIMainMenu.Tag)?.GetComponent<UIMainMenu>();
        if (MainMenu)
        {
            MainMenu.ReturnToPage(m_SavedPage);
        }
        else
        {
            Debug.Log("Can't find Main Menu Script");
        }
    }

    public void StartRecipe(SORecipe recipe)
    {
        currentRecipe = recipe;
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
        ReturnToMainMenuNonStopping();
        yield return new WaitUntil(() => SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Mainmenu"));
        float average = 0;
        i_scores.ForEach((int i) => { average += i; });
        average /= i_scores.Count;

        if (PlayerPrefs.HasKey(recipe.name))
        {
            if (PlayerPrefs.GetInt(recipe.name) < average)
                PlayerPrefs.SetInt(recipe.name, (int)average);
        } else
        {
            PlayerPrefs.SetInt(recipe.name, (int)average);
        }

        UIMainMenu MainMenu = GameObject.FindGameObjectWithTag(UIMainMenu.Tag)?.GetComponent<UIMainMenu>();
        if (MainMenu)
        {
            MainMenu.ShowEndRecipe((int)average);
        } else
        {
            Debug.Log("Can't find Main Menu Script");
        }

    }

    public void StartEndlessMode()
    {
        b_endless = true;
        StartCoroutine(EndlessModeCoroutine());
    }

    public IEnumerator EndlessModeCoroutine()
    {
        //Cycle through all available games in random order with increasing difficulty
        bool[] bPlayedArray = new bool[m_ScenesName.Count];
        for (int i = 0; i < bPlayedArray.Length; i++) bPlayedArray[i] = false;

        while (true)
        {
            //Check if I played all games, if true cleanup
            bool playedAllGame = true;
            for (int i = 0; i < m_ScenesName.Count; i++)
                if (bPlayedArray[i] == false)
                {
                    playedAllGame = false;
                    break;
                }

            if(playedAllGame)
                for (int i = 0; i < bPlayedArray.Length; i++) bPlayedArray[i] = false;


            int nextScene = -1;

            do
            {
                nextScene = Random.Range(0, m_ScenesName.Count);
            } while (bPlayedArray[nextScene]);

            bPlayedArray[nextScene] = true;

            RecipeEntry entryToLoad;
            entryToLoad.m_minigameScene = m_ScenesName[nextScene];
            Difficulty difficultyToPlay = (Difficulty)Random.Range(0, 3);
            entryToLoad.m_difficulty = difficultyToPlay;

            yield return StartCoroutine(_loadEntry(entryToLoad));

            yield return new WaitUntil(() => { return m_minigameManager.CanCloseGame; });

        }
    }

    public void SetReturnPage(PageToReturn page)
    {
        m_SavedPage = page;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
