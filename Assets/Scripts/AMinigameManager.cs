using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    EASY = 0, MEDIUM = 1, HARD = 2
};

public abstract class AMinigameManager : MonoBehaviour
{
    /// <summary>
    ///Abstract class that should contain the interface from which extend every minigameManager
    ///For good interface with GameManager
    /// </summary>

    protected Difficulty m_difficulty;
    public Difficulty Difficulty
    {
        get { return m_difficulty; }
        set { m_difficulty = value; }
    }

    protected bool b_gameFinished = false, b_canCloseGame = false;
    protected int i_score;

#region PROPERTIES
    public bool GameFinished
    {
        get { return b_gameFinished;}
    }
    public bool CanCloseGame
    {
        get { return b_canCloseGame; }
    }
    public int Score
    {
        get { return i_score;}
    }

#endregion

    //Score
    public abstract void BeginMinigame(Difficulty difficulty); //Shall contain the logic to begin the minigame;
    protected abstract void EndMinigame(); //Ends minigame and calculates score

    public virtual void PauseGame() { Time.timeScale = 0; }
    public virtual void ResumeGame() { Time.timeScale = 1; }

    public void SetReadyToClose() { b_canCloseGame = true; }
}
