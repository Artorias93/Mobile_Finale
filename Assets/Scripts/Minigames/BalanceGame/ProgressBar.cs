using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BalanceGame
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private BalanceGameManager balanceGameManager;
        [SerializeField]
        private bool isPlayer;

        public Image mask;

        private void Update()
        {
            GetFill();
        }
        private void GetFill() {
            float current=0;
            float max = 0;
            if (isPlayer)
            {
                current = balanceGameManager.f_currentWintime;
                max = balanceGameManager.m_gameParameters.f_winTimeSeconds;
            }
            else
            {
                current = balanceGameManager.f_currentLosetime;
                max = balanceGameManager.m_gameParameters.f_loseTImeSeconds;
            }
            float fillAmount = current / max;
            mask.fillAmount = fillAmount;
        }
    }
}