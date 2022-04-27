using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager m_instance;

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

        AMinigameManager aMinigameManager = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<AMinigameManager>();
        aMinigameManager.BeginMinigame(difficulty);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(minigameName));
    }
}
