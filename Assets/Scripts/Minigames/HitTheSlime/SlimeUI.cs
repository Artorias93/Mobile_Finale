using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SlimeMinigame
{
    public class SlimeUI : AUIManager
    {
        [SerializeField] private HitController m_hitController;

        private void Awake()
        {
            Assert.IsNotNull(m_hitController, "No hit controller given");
        }

        protected override void LateUpdate()
        {
            SlimeMinigameManager gm = SlimeMinigameManager.Instance;
            if (gm != null)
            {
                float normalizedTime = (float)gm.RemainingTime / gm.SelectedParameters.i_PlayTime;
                UpdateSlider(normalizedTime);

                int partialScore = (int)(3 * ((float)m_hitController.Hits / gm.SelectedParameters.i_requestedHits));
                partialScore = partialScore > 3 ? 3 : partialScore;
                scoreText.text = partialScore + "/3";

                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}

