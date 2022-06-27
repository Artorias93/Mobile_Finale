using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightCthulhu
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public int i_activeTentacles;
        public int i_grabbingTentacles;
        public int i_maxErrorCount;

    }

    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class MiniGameManager : AMinigameManager
    {
        public GameParameters GetGameParameters
        {
            get { return m_gameParameters; }
        }
        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }
        public int GetCurrentErrorCount
        {
            get { return cthulhu.i_ErrorCount; }
        }
        public bool b_GameFinished
        {
            get { return b_gameFinished; }
        }
        public static MiniGameManager Instance;
        public delegate void UpdateEvent();
        public static event UpdateEvent OnUpdate;

        [SerializeField] 
        public float frameTime = 0.5f;
        [SerializeField]
        private GameParameters gp_easy, gp_medium, gp_hard;
        [SerializeField]
        private PlayerManager player;
        [SerializeField]
        private CthulhuManager cthulhu;

        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Diff diff;


        private GameParameters m_gameParameters;
        private Timer m_gameTimer;
        private float f_elapsedTime=0;
        private bool b_gameStarted = false;

        // Start is called before the first frame update

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

        // Update is called once per frame
        void Update()
        {
            if (b_gameStarted) {
                f_elapsedTime += Time.deltaTime;
                if (f_elapsedTime > frameTime)
                {
                    f_elapsedTime -= frameTime;
                    if (OnUpdate != null)
                        OnUpdate();
                    if (cthulhu.i_ErrorCount >= m_gameParameters.i_maxErrorCount) {
                        cthulhu.i_ErrorCount = cthulhu.i_ErrorCount > m_gameParameters.i_maxErrorCount ? m_gameParameters.i_maxErrorCount : cthulhu.i_ErrorCount;
                        EndMinigame();
                    }
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

            player.InitializePlayer();
            cthulhu.InitializeCthulhu(m_gameParameters.i_activeTentacles, m_gameParameters.i_grabbingTentacles);
            m_gameTimer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            m_gameTimer.StartTimer();
            b_gameStarted = true;
        }

        protected override void EndMinigame()
        {
            b_gameFinished = true;
            m_gameTimer?.PauseTimer();
            int errorDifference = m_gameParameters.i_maxErrorCount - cthulhu.i_ErrorCount;
            errorDifference = errorDifference > 3 ? 3 : errorDifference;
            i_score = errorDifference;
        }
    }
}