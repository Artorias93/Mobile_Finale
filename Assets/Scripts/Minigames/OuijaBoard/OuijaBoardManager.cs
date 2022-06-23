using UnityEngine;

namespace OuijaBoardMinigame
{

    [System.Serializable]
    public struct GameParameters
    {
        public float m_playTime;
        public int m_wordsNum;
    }

    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class OuijaBoardManager : AMinigameManager
    {
        [Header("Game Parameters")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        private GameParameters m_gameParameters;

        [Header("References")]
        [SerializeField] private BoardManager m_ouijaManager;
        [SerializeField] private OuijaBoardUIManager m_ouijaBoardUIManager;

        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Diff diff;

        public static OuijaBoardManager m_instance;
        private Timer m_gameTimer;

        private bool b_win = false;

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
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public bool GameWon
        {
            get { return b_win; }
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
        private void Start()
        {
            if (b_enableDebug)
            {
                if (diff == Diff.easy) BeginMinigame(Difficulty.EASY);
                else if (diff == Diff.medium) BeginMinigame(Difficulty.MEDIUM);
                else if (diff == Diff.hard) BeginMinigame(Difficulty.HARD);
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

            m_gameTimer = Timer.Create(m_gameParameters.m_playTime, timerHasEnded, name: "gameTimer");
            m_gameTimer.StartTimer();
            m_ouijaManager.Initialize();
            m_ouijaManager.ProposeWord();
            i_score = 0;
            
        }

        protected override void EndMinigame()
        {
            m_ouijaManager.freezeSelector(true);
            b_gameFinished = true;
            i_score = (int)(3 * (float)i_score / m_gameParameters.m_wordsNum);
        }

        public void GameCompleted() { b_win = true; victoryBehaviour(); }

        public void wordGuessed() { i_score++; }

        private void timerHasEnded()
        {
            EndMinigame();
        }

        private void victoryBehaviour()
        {
            m_gameTimer.StopTimer();
            EndMinigame();
        }

        public override void PauseGame()
        {
            base.PauseGame();
            m_ouijaManager.freezeSelector(true);
        }

        public override void ResumeGame()
        {
            base.ResumeGame();
            m_ouijaManager.freezeSelector(false);
        }
    }
}
