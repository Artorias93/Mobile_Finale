using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FruitNinjaMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public float f_minDelay;
        public float f_maxDelay;
    }

    public class GameManager : AMinigameManager
    {
        [SerializeField] private GameObject obj_blade;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        [SerializeField] private EndGameScreenController endGameUI;

        private GameParameters gp_selectedParameters;
        private int i_outCount = 0;


        private Timer m_gameTimer;
        private bool m_gameFinished, m_gameWon;

#region PROPERTIES
        public bool GameFinished
        {
            get { return m_gameFinished; }
        }

        public bool GameWon
        {
            get { return m_gameWon; }
        }

        public int OutCount
        {
            get { return i_outCount; }
        }

        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public GameParameters GameParameters
        {
            get { return gp_selectedParameters; }
        }

#endregion

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            m_gameWon = false;
            m_gameFinished = false;
        }

        public void increaseErrorCount()
        {
            i_outCount++;
            if (i_outCount > 3)
            {
                i_outCount = 3;
                EndMinigame();
            }
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case (Difficulty.EASY):
                    gp_selectedParameters = gp_easy;
                    break;
                case (Difficulty.MEDIUM):
                    gp_selectedParameters = gp_medium;
                    break;
                case (Difficulty.HARD):
                    gp_selectedParameters = gp_hard;
                    break;
            }

            m_gameTimer = Timer.Create(gp_selectedParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            Spawner.Instance.MinDelay = gp_selectedParameters.f_minDelay;
            Spawner.Instance.MaxDelay = gp_selectedParameters.f_maxDelay;
            m_gameTimer.StartTimer();
        }


        public override void EndMinigame()
        {
            if (m_gameTimer != null && m_gameTimer.RemainingTime > 0.0f && i_outCount < 3)
                m_gameWon = true;
            m_gameFinished = true;
            Spawner.Instance.Stop();
        }

        public override void PauseGame()
        {
            throw new System.NotImplementedException();
        }

        public override void ResumeGame()
        {
            throw new System.NotImplementedException();
        }
    }
}
