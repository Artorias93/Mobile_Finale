using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;



namespace OuijaBoardMinigame
{

    [System.Serializable]
    public struct GameParameters
    {
        public float m_playTime;
        public int m_wordsNum;
    }

    public class OuijaBoardManager : AMinigameManager
    {
        [Header("Game Parameters")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        private GameParameters m_gameParameters;

        [Header("References")]
        [SerializeField] private BoardManager m_ouijaManager;
        [SerializeField] private OuijaBoardUIManager m_ouijaBoardUIManager;

        public static OuijaBoardManager m_instance;
        private Timer gameTimer;

#region PROPERTIES

        public float PlayTime
        {
            get { return m_gameParameters.m_playTime; }
        }
        public int WordsNum
        {
            get { return m_gameParameters.m_wordsNum; }
        }

        public float RemainingTime
        {
            get { return gameTimer ? gameTimer.RemainingTime : -1; }
        }
#endregion
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
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;

            if (m_difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (m_difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (m_difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            gameTimer = Timer.Create(m_gameParameters.m_playTime, timerHasEnded, name: "gameTimer");
            gameTimer.StartTimer();
        }

        public override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }

        private void timerHasEnded()
        {
            Debug.Log("TEMPO FINITOOO!!!");
        }

        private void victoryBehaviour()
        {
            Debug.Log("HAI VINTOOO!!!");
        }

    }
}
