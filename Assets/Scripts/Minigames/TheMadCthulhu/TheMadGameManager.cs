using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheMadCthulhuMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_Lives;
        public int i_Time;
        public GameObject[] arrgo_Enemy;
        
    }

    public class TheMadGameManager : AMinigameManager
    {
       
        
        [Header("Difficulty Settings")]
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        private GameParameters gp_selectedParameters;

        [Header("References")]
        [SerializeField] private PlayerController PlayerController;
            
        
        private Timer m_gameTimer;
        public static TheMadGameManager Instance;
        

    #region PROPERTIES
        public float remainingTime
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

                
        }
        private void Start()
        {
            BeginMinigame(Difficulty.HARD);
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

            m_gameTimer = Timer.Create(gp_selectedParameters.i_Time, LoseMinigame, name: "GameTimer");
            m_gameTimer.StartTimer();

            SetUpTheEnemy(gp_selectedParameters);
        }

        private void SetUpTheEnemy(GameParameters gp_selectedParameters)
        {
            for (int i = 0; i < gp_selectedParameters.arrgo_Enemy.Length; i++)
            {
                gp_selectedParameters.arrgo_Enemy[i].SetActive(true);
            }
        }


        protected override void EndMinigame()
        {
                
            b_gameFinished = true;

        }
        public void LoseMinigame()
        {
            i_score = 0;
            EndMinigame();
        }

        public void WinMinigame()
        {

            float percentageHits = (float)PlayerController.Lives / gp_selectedParameters.i_Lives;
            i_score = (int)(3f * Mathf.Clamp(percentageHits, 0f, 1f));
            EndMinigame();


        }
    }


}



