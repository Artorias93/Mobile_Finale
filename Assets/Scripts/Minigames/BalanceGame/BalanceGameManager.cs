using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BalanceGame
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public float f_rescaleRate;
        public float f_deltaScale;
        public float f_winTimeSeconds;
        public float f_loseTImeSeconds;
        public float f_initialGravityScale;
        public float f_stopRescaleSeconds;
    }

    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class BalanceGameManager : AMinigameManager
    {
        public GameParameters m_gameParameters;
        public static BalanceGameManager Instance;
        public float f_currentWintime
        {
            get { return m_player.f_ContactTime; }
        }

        public float f_currentLosetime
        {
            get { return m_gameWeight.f_ContactTime; }
        }
        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public bool b_GameFInished
        {
            get { return b_gameFinished; }
        }

        [SerializeField]
        private PlayerWeight m_player;
        [SerializeField]
        private GameWeight m_gameWeight;
        [SerializeField]
        private Rigidbody2D m_balanceRigidBody;

        [SerializeField]
        private GameParameters gp_easy, gp_medium, gp_hard;

        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Diff diff;

        private bool b_gameWon = false;
        private Timer m_gameTimer;
        private bool gameStarted=false;

        private void Awake()
        {
            Instance = this;
            i_score = 0;
            b_gameFinished = false;
        }
        void Start()
        {
            if (b_enableDebug)
            {
                if (diff == Diff.easy) BeginMinigame(Difficulty.EASY);
                else if (diff == Diff.medium) BeginMinigame(Difficulty.MEDIUM);
                else if (diff == Diff.hard) BeginMinigame(Difficulty.HARD);
            }
        }

        void Update()
        {
            if (gameStarted && !b_gameFinished) {
                if (m_gameWeight.f_ContactTime >= m_gameParameters.f_loseTImeSeconds)
                {
                    b_gameWon = false;
                    EndMinigame();
                }
                else if (m_player.f_ContactTime >= m_gameParameters.f_winTimeSeconds)
                {
                    b_gameWon = true;
                    EndMinigame();
                }
            }

        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            if (difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;

            m_player.InitializeGameWeight(m_gameParameters.f_deltaScale, m_gameParameters.f_initialGravityScale, m_gameParameters.f_rescaleRate);
            m_gameWeight.InitializeGameWeight(m_gameParameters.f_deltaScale, m_gameParameters.f_initialGravityScale, m_gameParameters.f_stopRescaleSeconds, m_gameParameters.f_rescaleRate);
            m_gameTimer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            m_gameTimer.StartTimer();
            gameStarted = true;
        }

        protected override void EndMinigame()
        {
            m_balanceRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            m_player.StopAnimation();
            m_gameWeight.StopAnimation();
            m_gameTimer?.PauseTimer();
            b_gameFinished = true;
            if (b_gameWon)
            {
                float decimalTime = f_currentLosetime / m_gameParameters.f_loseTImeSeconds;
                decimalTime = 1 - decimalTime + 0.14f;
                i_score = (int)(3 *decimalTime);
            }
        }
    }
}