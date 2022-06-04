using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightCthulhu
{
    public class CthulhuUIManager : AUIManager
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        protected override void LateUpdate()
        {
            MiniGameManager gm = MiniGameManager.Instance;
            if (gm != null)
            {
                float normalizedTime = (float)gm.RemainingTime / gm.GetGameParameters.f_gameTime;
                UpdateSlider(normalizedTime);
                scoreText.text = gm.GetCurrentErrorCount+"/" + gm.GetGameParameters.i_maxErrorCount;
                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}