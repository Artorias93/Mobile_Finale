using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FruitNinjaMinigame
{
    public class UIManager : AUIManager
    {

        protected override void LateUpdate()
        {
            GameManager gm = GameManager.Instance;
            if (gm == null) return;

            if (gm.GameFinished)
            {
                endGameScreenController.showEndGamePanel(gm.Score);
            }
            else if (gm.RemainingTime >= 0)
            {
                float normalizedTime = gm.RemainingTime / gm.GameParameters.f_gameTime;
                UpdateSlider(normalizedTime);

                scoreText.text = gm.OutCount + "/" + 3;


            }
        }
    }
}