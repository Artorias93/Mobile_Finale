using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheMadCthulhuMinigame
{
    public class UITheMadCthulhu : AUIManager
    {
  
        [SerializeField] private PlayerController m_playerController;
        [SerializeField]
        private TheMadGameManager gameManager;

        protected override void LateUpdate()
        {
            gameManager = TheMadGameManager.Instance;
            if (gameManager != null)
            {
                float normalizedTime = gameManager.remainingTime / gameManager.SelectedParameters.i_Time;
                UpdateSlider(normalizedTime);
                
                scoreText.text = m_playerController.Lives + "/" + gameManager.SelectedParameters.i_Lives;

            }

            if (gameManager.GameFinished)
            {
                endGameScreenController.showEndGamePanel(gameManager.Score);
            }
        }

    }
}
