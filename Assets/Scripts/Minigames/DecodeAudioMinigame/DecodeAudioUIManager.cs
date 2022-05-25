using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DecodeAudioMinigame
{
    public class DecodeAudioUIManager : AUIManager
    {

        protected override void LateUpdate()
        {
            DecodeAudioManager dam = DecodeAudioManager.m_instance;

            scoreText.text = dam.currentTurn + "/" + dam.TurnNumber;
            if (dam.GameFinished)
            {
                endGameScreenController.showEndGamePanel(dam.Score);
            }
            else
            {
                float normalizedTime = (float)dam.RemainingTime / dam.PlayTime;
                UpdateSlider(normalizedTime);               
            }

            
        }
    }
}