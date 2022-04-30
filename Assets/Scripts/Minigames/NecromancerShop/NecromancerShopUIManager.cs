using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NecromancerShop
{
    public class NecromancerShopUIManager : AUIManager
    {
        [SerializeField]
        private List<Sprite> sprites;
        [SerializeField]
        private GameObject m_IngredientsContainer;
        [SerializeField]
        private GameObject m_IngredientsListPanel;
        [SerializeField]
        private GameObject m_IngredientsList;
        [SerializeField]
        private GameObject m_ingredientPrefab;
        [SerializeField]
        private Text t_remainingOpenListCount;
        [SerializeField]
        private NecromancerShopGM necromancerShopGM;

        public List<GameObject> InitializeIngredients(int i_ingredientsCount)
        {
            List<GameObject> m_totalIngredients=new List<GameObject>();
            for (int i = 0; i < i_ingredientsCount; i++)
            {
                GameObject go = Instantiate(m_ingredientPrefab, m_IngredientsContainer.transform);
                go.GetComponent<Image>().sprite= sprites[Random.Range(0, sprites.Count)];
                m_totalIngredients.Add(go);
            }
            setRemainingOpenListCount(necromancerShopGM.m_gameParameters.i_remainingOpenListCount);
            return m_totalIngredients;
        }

        private void setRemainingOpenListCount(int i_remainingOpensCount) {
            t_remainingOpenListCount.text = i_remainingOpensCount.ToString();
        }

        public void showIngredientsList() {
            if (necromancerShopGM.m_gameParameters.i_remainingOpenListCount > 0) {
                necromancerShopGM.m_gameParameters.i_remainingOpenListCount--;
                setRemainingOpenListCount(necromancerShopGM.m_gameParameters.i_remainingOpenListCount);
                m_IngredientsListPanel.SetActive(true);

            }
        }

        public void addCorrectIngredientsToList(List<GameObject> m_ingredients) {
            for (int i = 0; i < m_ingredients.Count; i++) {
                GameObject go=Instantiate(m_ingredients[i], m_IngredientsList.transform);
                DraggableItem draggable = go.GetComponent<DraggableItem>();
                draggable.setUnchecked();
                go.name = m_ingredients[i].name + "L";
                m_ingredients[i].GetComponent<DraggableItem>().listElement = draggable;
            }
        }

        public void hideIngredientsList() {
            m_IngredientsListPanel.SetActive(false);
        }

        protected override void LateUpdate()
        {
            //aggiungere
        }
    }
}
