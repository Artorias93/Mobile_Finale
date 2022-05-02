using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace OuijaBoardMinigame
{
    public class OuijaBoardUIManager : AUIManager
    {
        [Header("References")]
        public Text clearedWords;
        public GameObject target;
        public GameObject results;

        protected override void LateUpdate()
        {
            OuijaBoardManager obm = OuijaBoardManager.m_instance;

            if(obm != null)
            {
                float normalizedTime = (float)obm.RemainingTime / obm.PlayTime;
                UpdateSlider(normalizedTime);
                if (obm.GameFinished) {
                    endGameScreenController.showEndGamePanel(obm.b_win);
                }
            }
            
        }
    }
}