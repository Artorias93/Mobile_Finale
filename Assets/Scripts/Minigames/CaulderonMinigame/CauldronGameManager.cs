using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CouldronMinigame 
{ 
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public float changeSecondsSpeedController;
        public float maxTimeInError;
        public float max_lives;
        public float spawnTimeSeconds;
    }
    public class CauldronGameManager : AMinigameManager
    {
        [SerializeField]
        public GameParameters gp_easy, gp_medium, gp_hard;
        public GameParameters m_gameParameters;
        private Difficulty difficulty1;
        private Timer m_timer;

        public bool b_gameWon=false;

        IngredientController ingredientController;
        SpeedController speedController;
        public float remaningTime
        {
            get { return m_timer ? m_timer.RemainingTime : -1; }

        }

        public static CauldronGameManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        public override void BeginMinigame(Difficulty difficulty)
        {
            difficulty1=difficulty;
            if (difficulty1 == Difficulty.EASY)
            {
                m_gameParameters = gp_easy;
            }
            else if(difficulty1 == Difficulty.MEDIUM)
            {
                m_gameParameters = gp_medium;
            }
            else if (difficulty1 == Difficulty.HARD)
            {
                m_gameParameters = gp_hard;
            }
            m_timer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame,destroyOnFinish:true,name:"timer");
            m_timer.StartTimer();
            

        }

      

        // Start is called before the first frame update
        void Start()
        {
            BeginMinigame(Difficulty.EASY);
        }  

        protected override void EndMinigame()
        {
            //i_score = blablabla
            //stop ladle
            m_timer?.StopTimer();
            b_gameFinished = true;
            
        }

        public void LoseMinigame()
        {
            //lose logic
            b_gameWon = false;
            //...
            EndMinigame();
        }

        public void WinMinigame()
        {
            b_gameWon = true;
            //...
            EndMinigame();
        }


        public override void PauseGame()
        {
            base.PauseGame();
            //stop cose spawn oggetti, movimento slider, tempo
            
        }

        public override void ResumeGame()
        {
            base.ResumeGame();
            if (m_timer != null && m_timer.RemainingTime > 0.0f && ingredientController.Lives > 0 && speedController.TimeinError < m_gameParameters.maxTimeInError) { }
        }
    }
}
        
   