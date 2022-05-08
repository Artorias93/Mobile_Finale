using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawingRunes
{
    public class UIManager : AUIManager
    {
        protected override void LateUpdate()
        {
            DrawingGameManager gm = DrawingGameManager.Instance;
            if (gm != null)
            {
                float normalizedTime = (float) gm.RemainingTime / gm.GameParameters.i_playTime;
                UpdateSlider(normalizedTime);

                scoreText.text = gm.CurrentTurn + "/" + gm.GameParameters.i_maxTurns;

                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}

