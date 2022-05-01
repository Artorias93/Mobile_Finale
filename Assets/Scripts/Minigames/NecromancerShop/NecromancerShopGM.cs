using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public class NecromancerShopGM : AMinigameManager
    {
        [SerializeField]
        private NecromancerShopUIManager m_uiManager;
        [SerializeField] 
        private GameParameters gp_easy, gp_medium, gp_hard;
        [SerializeField]
        private List<ObstacleSpawner> m_obstacleSpawners;
        public int i_CurrentErrorCount
        {
            get { return i_currentErrorCount; }
        }
        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public static NecromancerShopGM Instance;
        public GameParameters m_gameParameters;


        private List<GameObject> m_totalIngredients;
        private List<GameObject> m_correctIngredients=new List<GameObject>();
        private int i_currentCorrectsCount=0,i_currentErrorCount=0;
        private Timer m_gameTimer;



        void Awake() {
            Instance = this;
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
            m_totalIngredients= m_uiManager.InitializeIngredients(m_gameParameters.i_maxIngredientsCount);
            m_gameTimer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            m_gameTimer.StartTimer();
            AddCorrectIngredients();
            InitializeObstacleSpawners();
        }

        private void InitializeObstacleSpawners() {
            for (int i = 0; i < m_obstacleSpawners.Count; i++)
            {
                m_obstacleSpawners[i].InitializeSpawner(m_gameParameters.i_minSecondsStopObstacle, m_gameParameters.i_maxSecondsStopObstacle);
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

        public override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {
            BeginMinigame(Difficulty.EASY);
        }
        public void updateCorrectCounter()
        {
            i_currentCorrectsCount++;
            if (i_currentCorrectsCount == m_gameParameters.i_correctIngredientsCount) {
                //EndMinigame();
                Debug.LogWarning("WIN!!");
            }
        }

        public void updateErrorCounter()
        {
            i_currentErrorCount++;
            if (m_gameParameters.i_maxErrorsCount <= i_currentErrorCount)
            {
                //EndMinigame();
                Debug.LogWarning("LOST!!");
            }
        }
    }
}
