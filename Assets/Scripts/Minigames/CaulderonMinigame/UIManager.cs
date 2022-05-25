using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CouldronMinigame
{

    public class UIManager : AUIManager
    {
        [SerializeField]
        private IngredientController ingredientController;
        
        private CauldronGameManager gameManager;

        protected override void LateUpdate()
        {
            gameManager = CauldronGameManager.Instance;
            if (gameManager != null)
            {
                float normalizedTime = gameManager.remaningTime/gameManager.m_gameParameters.f_gameTime;
                UpdateSlider(normalizedTime);
                scoreText.text = ingredientController.Lives+"/"+gameManager.m_gameParameters.max_lives;
                
            }

            if(gameManager.b_gameFinished)
            {
                endGameScreenController.showEndGamePanel(gameManager.b_gameWon);
            }
        }

     
        

    }

}
