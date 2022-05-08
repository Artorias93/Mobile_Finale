using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NecromancerShop
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public int i_maxIngredientsCount;
        public int i_correctIngredientsCount;
        public int i_maxErrorsCount;
        public int i_remainingOpenListCount;
        public int i_minSecondsStopObstacle;
        public int i_maxSecondsStopObstacle;
        public int i_activeSpawnersCounter;
    }
    public class NecromancerShopGM : AMinigameManager
    {
        [SerializeField]
        private NecromancerShopUIManager m_uiManager;
        [SerializeField] 
        private GameParameters gp_easy, gp_medium, gp_hard;
        [SerializeField]
        private List<ObstacleSpawner> m_obstacleSpawners;
        [SerializeField]
        private GeneralUiManager m_GeneralUI;
        [SerializeField]
        private List<Image> cartPositionImages;
        public int i_CurrentErrorCount
        {
            get { return i_currentErrorCount; }
        }
        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }
        public bool b_WinCondition
        {
            get { return b_winCondition; }
        }
        public bool b_GameFinished
        {
            get { return b_gameFinished; }
        }
        public static NecromancerShopGM Instance;
        public GameParameters m_gameParameters;
        public int i_inizializedSpawners = 0;

        private List<GameObject> m_totalIngredients;
        private List<GameObject> m_correctIngredients=new List<GameObject>();
        private int i_currentCorrectsCount=0,i_currentErrorCount=0;
        private Timer m_gameTimer;
        private bool b_winCondition=false,b_gameFinished=false;
       



        void Awake() {
            Instance = this;
        }
        void Start()
        {
            //Remove comment for debug single scene
            BeginMinigame(Difficulty.HARD);
        }
        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            m_gameParameters.i_activeSpawnersCounter = m_gameParameters.i_activeSpawnersCounter <= m_obstacleSpawners.Count ? m_gameParameters.i_activeSpawnersCounter : m_obstacleSpawners.Count;
            if (m_difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (m_difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (m_difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;
            m_totalIngredients= m_uiManager.InitializeIngredients(m_gameParameters.i_maxIngredientsCount);
            m_gameTimer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            m_gameTimer.StartTimer();
            AddCorrectIngredients();
            InitializeObstacleSpawners();
        }

        private void InitializeObstacleSpawners() {
            while (i_inizializedSpawners < m_gameParameters.i_activeSpawnersCounter) {
                ObstacleSpawner spawner = m_obstacleSpawners[Random.Range(0, m_obstacleSpawners.Count)];
                if (!spawner.b_isInitialized) { 
                    spawner.InitializeSpawner(m_gameParameters.i_minSecondsStopObstacle, m_gameParameters.i_maxSecondsStopObstacle);
                }
            }
        }
        private void AddCorrectIngredients()
        {
            while (m_correctIngredients.Count < m_gameParameters.i_correctIngredientsCount)
            {
                int i_range = Random.Range(0, m_totalIngredients.Count);
                GameObject correctIngredient = m_totalIngredients[i_range];
                if (!m_correctIngredients.Contains(correctIngredient))
                {
                    m_correctIngredients.Add(correctIngredient);
                    correctIngredient.GetComponent<DraggableItem>().b_IsIngredient = true;
                    correctIngredient.name = "Ingredient"+i_range;
                }
            }
            m_uiManager.addCorrectIngredientsToList(m_correctIngredients);
        }
        private void Update()
        {
            if (!b_GameFinished) {
                InitializeObstacleSpawners();
            }
        }
        public override void EndMinigame()
        {
           
            if (m_gameTimer != null && m_gameTimer.RemainingTime <= 0.0f)
            {
                b_winCondition = false;
                
            }
            m_GeneralUI.DisableAll();

            for (int i = 0; i < m_totalIngredients.Count; i++)
            {
                m_totalIngredients[i].GetComponent<DraggableItem>().stopDraggable();
            }

            for (int i = 0; i < m_obstacleSpawners.Count; i++) {
                m_obstacleSpawners[i].StopSpawner();
            }

            b_gameFinished = true;
            m_gameTimer.StopTimer();
        }

        public void updateCorrectCounter(Sprite sprite)
        {
            i_currentCorrectsCount++;
            if (cartPositionImages.Count > 0 && (m_gameParameters.i_correctIngredientsCount - i_currentCorrectsCount < cartPositionImages.Count))  {
                Color tmpColor = cartPositionImages[0].color;
                tmpColor.a = 1;
                cartPositionImages[0].color = tmpColor;
                cartPositionImages[0].sprite = sprite;
                cartPositionImages.RemoveAt(0);
            }
            if (i_currentCorrectsCount == m_gameParameters.i_correctIngredientsCount) {
                EndMinigame();
                b_winCondition = true;
            }
        }

        public void updateErrorCounter()
        {
            i_currentErrorCount++;
            if (m_gameParameters.i_maxErrorsCount <= i_currentErrorCount)
            {
                EndMinigame();
                b_winCondition = false;
            }
        }
    }
}
