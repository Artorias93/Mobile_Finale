using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NecromancerShop
{
    public class GeneralUiManager : AUIManager
    {
        private NecromancerShopGM gamManager;

        protected override void LateUpdate()
        {
            gamManager = NecromancerShopGM.Instance;
            if (gamManager != null) {
                float normalizedTime = gamManager.RemainingTime / gamManager.m_gameParameters.f_gameTime;
                UpdateSlider(normalizedTime);
                scoreText.text = gamManager.i_CurrentErrorCount + "/" + gamManager.m_gameParameters.i_maxErrorsCount;
                if (gamManager.b_GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gamManager.Score);
                }
            }
        }
    }
}
