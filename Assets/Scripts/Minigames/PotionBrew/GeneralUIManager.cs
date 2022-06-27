using System.Collections;
using System.Collections.Generic;
using PotionBrewMinigame;
using UnityEngine;

namespace PotionBrewMinigame
{
    public class GeneralUIManager : AUIManager
    {
        protected override void LateUpdate()
        {
            PotionBrewManager gm = PotionBrewManager.Instance;
            if (gm != null)
            {
                float normalizedTime = gm.RemainingTime / gm.GameParameters.f_gameTime;
                UpdateSlider(normalizedTime);
                int stepCount = gm.CurrentStepCount;
                //stepCount++;
                scoreText.text = stepCount + "/" + gm.GameParameters.i_maxStepsCount;

                if (gm.CurrentStep.Equals(PotionBrewManager.STEPS.FINISH))
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}
