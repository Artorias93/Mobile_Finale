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

                float LivesToShow = Mathf.Max(m_playerController.Lives, 0);

                scoreText.text = LivesToShow + "/" + gameManager.SelectedParameters.i_Lives;

            }

            if (gameManager.GameFinished)
            {
                endGameScreenController.showEndGamePanel(gameManager.Score);
            }
        }

    }
}
