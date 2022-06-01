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
        private Timer m_timer;

        [SerializeField]
        IngredientController ingredientController;
        [SerializeField]
        SpeedController speedController;
        public float remaningTime
        {
            get { return m_timer ? m_timer.RemainingTime : -1; }

        }

        public static CauldronGameManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }
        
        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            if (m_difficulty == Difficulty.EASY)
            {
                m_gameParameters = gp_easy;
            }
            else if(m_difficulty == Difficulty.MEDIUM)
            {
                m_gameParameters = gp_medium;
            }
            else if (m_difficulty == Difficulty.HARD)
            {
                m_gameParameters = gp_hard;
            }
            m_timer = Timer.Create(m_gameParameters.f_gameTime, WinMinigame ,destroyOnFinish:true,name:"timer");
            m_timer.StartTimer();

            speedController.BeginPlay();
            

        }

      

        // Start is called before the first frame update
        void Start()
        {
            //BeginMinigame(Difficulty.EASY);
        }  

        protected override void EndMinigame()
        {

            m_timer?.StopTimer();
            b_gameFinished = true;
        }

        public void LoseMinigame()
        {
                i_score = 0;
                EndMinigame();
        }

        public void WinMinigame()
        {

            float percentageTime = (float)speedController.TimeinError / m_gameParameters.maxTimeInError;
            float percentageLives = (float)ingredientController.Lives / m_gameParameters.max_lives;

            float percentage = (percentageLives + percentageTime)/2;
            i_score = (int)(3f * Mathf.Clamp(percentage, 0f, 1f));
            EndMinigame();
        
            
        }
    }
}
        
   