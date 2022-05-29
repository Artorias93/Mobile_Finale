using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeDungeonMinigame
{

    public class EscapeTheDungeonUI : AUIManager
    {
        protected override void LateUpdate()
        {
            EscapeMinigameManager emm = EscapeMinigameManager.Instance;
            if (emm != null)
            {
                float normalizedTime = (float)emm.RemainingTime / emm.SelectedParameters.f_PlayTime;
                UpdateSlider(normalizedTime);


                if (emm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(emm.Score);
                }
            }
        }
    }
}
