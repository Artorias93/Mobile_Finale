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
        public int i_remainingErrorsCount;
        public int i_remainingOpenListCount;
    }
    public class NecromancerShopGM : AMinigameManager
    {
        [SerializeField]
        private NecromancerShopUIManager m_uiManager;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;


        public GameParameters m_gameParameters;
        private List<GameObject> m_totalIngredients;
        private List<GameObject> m_correctIngredients=new List<GameObject>();
        private int i_currentCorrectsCount=0;


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
            AddCorrectIngredients();
           
        }

        private void AddCorrectIngredients()
        {
            while (m_correctIngredients.Count < m_gameParameters.i_correctIngredientsCount)
            {
                GameObject correctIngredient = m_totalIngredients[Random.Range(0, m_totalIngredients.Count)];
                if (!m_correctIngredients.Contains(correctIngredient))
                {
                    m_correctIngredients.Add(correctIngredient);
                    correctIngredient.GetComponent<DraggableItem>().b_IsIngredient = true;
                    correctIngredient.name = "Ingredient";
                }
            }
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
