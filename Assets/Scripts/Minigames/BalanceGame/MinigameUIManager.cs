using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BalanceGame
{
    public class MinigameUIManager : AUIManager
    {

        [SerializeField]
        private Text playerTipText;
        [SerializeField]
        private Image playerTipImage;
        private float deltaFadeValue = 0.1f;

        private void Start()
        {
            HideScore();
            StartCoroutine(fadeOut());
        }

        private IEnumerator fadeOut()
        {
            yield return new WaitForSeconds(2.0f);
            float i = 1;
            do
            {
                i -= deltaFadeValue;
                Color tmpColor = playerTipImage.color;
                tmpColor.a = i;
                playerTipImage.color = tmpColor;

                Color tmpColorText = playerTipText.color;
                tmpColorText.a = i;
                playerTipText.color = tmpColorText;
                yield return new WaitForSeconds(deltaFadeValue / 2);
            } while (i >= 0);
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