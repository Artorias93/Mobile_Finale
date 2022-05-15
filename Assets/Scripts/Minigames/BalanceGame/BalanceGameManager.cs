using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BalanceGame
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public float f_deltaScale;
        public float f_winTimeSeconds;
        public float f_loseTImeSeconds;

    }
    public class BalanceGameManager : AMinigameManager
    {   [SerializeField]
        private PlayerWeight m_player;
        [SerializeField]
        private GameWeight m_gameWeight;
        [SerializeField]
        private GameParameters gp_easy, gp_medium, gp_hard;

        public GameParameters m_gameParameters;
        public static BalanceGameManager Instance;

        private Timer m_gameTimer;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            BeginMinigame(Difficulty.EASY);
        }

        void Update()
        {

        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            if (difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;
            m_gameTimer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            //            m_gameTimer.StartTimer();
            m_player.InitializeGameWeight(m_gameParameters.f_deltaScale);
            m_gameWeight.InitializeGameWeight(m_gameParameters.f_deltaScale);

        }

        protected override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }
    }
}