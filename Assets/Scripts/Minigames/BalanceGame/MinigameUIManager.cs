using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BalanceGame
{
    public class MinigameUIManager : AUIManager
    {

        private void Start()
        {
            HideScore();
        }
        protected override void LateUpdate()
        {
            BalanceGameManager gm = BalanceGameManager.Instance;
            if (gm != null)
            {
                float normalizedTime = (float)gm.RemainingTime / gm.m_gameParameters.f_gameTime;
                UpdateSlider(normalizedTime);
                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}