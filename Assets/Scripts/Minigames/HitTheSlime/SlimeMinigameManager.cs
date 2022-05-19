using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace SlimeMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_requestedHits;
        public int i_PlayTime;
    }

    public class SlimeMinigameManager : AMinigameManager
    {
        [Header("Difficulty Settings")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        private GameParameters gp_selectedParameters;

        [Header("References")]
        [SerializeField] private HitController m_hitController;

        private Timer m_gameTimer;
        public static SlimeMinigameManager Instance;

        #region PROPERTIES
        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public GameParameters SelectedParameters
        {
            get { return gp_selectedParameters; }
        }
        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            Assert.IsNotNull(m_hitController, "Can't find Hit Controller");
        }
        public override void BeginMinigame(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.EASY: gp_selectedParameters = gp_easy; break;
                case Difficulty.MEDIUM: gp_selectedParameters = gp_medium; break;
                case Difficulty.HARD: gp_selectedParameters = gp_hard; break;
                default: Debug.Log("Difficulty Check failed, fallback to Easy"); gp_selectedParameters = gp_easy; break;
            }

            m_gameTimer = Timer.Create(gp_selectedParameters.i_PlayTime, EndMinigame, name: "GameTimer");
            m_gameTimer.StartTimer();
        }

        protected override void EndMinigame()
        {
            //Check if I have enough hits and decide score based on percentage
            b_gameFinished = true;

            float percentageHits = (float) m_hitController.Hits / gp_selectedParameters.i_requestedHits;
            i_score = (int) (3f * Mathf.Clamp(percentageHits, 0f, 1f));
        }
    }

}

