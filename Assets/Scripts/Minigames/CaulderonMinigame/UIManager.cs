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
                float normalizedTime = gameManager.remaningTime/gameManager.SelectedParameters.f_gameTime;
                UpdateSlider(normalizedTime);
                scoreText.text = ingredientController.Lives+"/"+gameManager.SelectedParameters.max_lives;
                
            }

            if(gameManager.GameFinished)
            {
                endGameScreenController.showEndGamePanel(gameManager.Score);
            }
        }

     
        

    }

}
