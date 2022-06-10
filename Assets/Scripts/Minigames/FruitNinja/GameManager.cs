using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FruitNinjaMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_playTime;
        public int i_maxError;
        public float f_minDelay;
        public float f_maxDelay;
    }
    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class GameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private GameObject obj_blade;
        [SerializeField] private EndGameScreenController endGameUI;

        [Header("Game Parameters")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        
        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Diff diff;

        private GameParameters m_gameParameters;
        private int i_outCount = 0;


        private Timer m_gameTimer;
        private bool m_gameWon;

#region PROPERTIES

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
            get { return m_gameParameters; }
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
            b_gameFinished = false;
        }
        private void Start()
        {
            if (b_enableDebug)
            {
                if (diff == Diff.easy) BeginMinigame(Difficulty.EASY);
                else if (diff == Diff.medium) BeginMinigame(Difficulty.MEDIUM);
                else if (diff == Diff.hard) BeginMinigame(Difficulty.HARD);
            }
        }

        public void increaseErrorCount()
        {
            i_outCount++;
            if (i_outCount >= m_gameParameters.i_maxError)
            {
                i_outCount = m_gameParameters.i_maxError;
                EndMinigame();
            }
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case (Difficulty.EASY):
                    m_gameParameters = gp_easy;
                    break;
                case (Difficulty.MEDIUM):
                    m_gameParameters = gp_medium;
                    break;
                case (Difficulty.HARD):
                    m_gameParameters = gp_hard;
                    break;
            }

            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            Spawner.Instance.MinDelay = m_gameParameters.f_minDelay;
            Spawner.Instance.MaxDelay = m_gameParameters.f_maxDelay;
            m_gameTimer.StartTimer();
        }


        protected override void EndMinigame()
        { 
            m_gameTimer?.StopTimer();
            b_gameFinished = true;
            i_score = 3 - i_outCount;
            Spawner.Instance.Stop();
        }
        private void VictoryBehaviour()
        {
            m_gameWon = true;
            EndMinigame();
        }
        private void onTimeEnd()
        {
            VictoryBehaviour();
        }

        public override void PauseGame()
        {
            base.PauseGame();
            Spawner.Instance.Stop();
        }

        public override void ResumeGame()
        {
            base.ResumeGame();
            Spawner.Instance.Start();
        }
    }
}
