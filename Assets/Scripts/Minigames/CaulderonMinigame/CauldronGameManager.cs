using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CouldronMinigame 
{ 
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        
        public float maxTimeInError;
        public float max_lives;
        
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
            m_timer = Timer.Create(m_gameParameters.f_gameTime, WinMinigame ,destroyOnFinish:true,name:"timer");
            m_timer.StartTimer();
            

        }

      

        // Start is called before the first frame update
        void Start()
        {
            BeginMinigame(Difficulty.EASY);
        }  

        protected override void EndMinigame()
        {
            
            m_timer?.StopTimer();
            b_gameFinished = true;
        }

        public void LoseMinigame()
        {
            if (ingredientController.Lives == 0 || speedController.TimeinError == 0)
            {
                b_gameWon = false;
                i_score = 0;
                //...
                EndMinigame();

            }
        }

        public void WinMinigame()
        {
            b_gameWon = true;
            float percentageTime = (float)speedController.TimeinError / m_gameParameters.maxTimeInError;
            float percentageLives = (float)ingredientController.Lives / m_gameParameters.max_lives ;
            int i_scoreTemp = (int)(3f * Mathf.Clamp(percentageTime, 0f, 1f));
            i_scoreTemp += (int)(3f * Mathf.Clamp(percentageLives, 0f, 1f));
            i_score = i_scoreTemp / 2;
            
            //...
            EndMinigame();
        }
    }
}
        
   