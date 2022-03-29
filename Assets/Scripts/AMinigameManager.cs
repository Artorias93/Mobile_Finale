using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMinigameManager : MonoBehaviour
{
    /// <summary>
    ///Abstract class that should contain the interface from which extend every minigameManager
    ///For good interface with GameManager
    /// </summary>

    private Difficulty m_difficulty;
    public Difficulty Difficulty
    {
        get { return m_difficulty; }
        set { m_difficulty = value; }
    }

    public abstract void BeginMinigame(Difficulty difficulty); //Shall contain the logic to begin the minigame;
    public abstract void EndMinigame(); //Shall contain the logic to give back control to the gameManager;
}
