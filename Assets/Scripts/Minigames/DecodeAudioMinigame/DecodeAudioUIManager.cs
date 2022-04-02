using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DecodeAudioMinigame
{
    public class DecodeAudioUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Text turnText;
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Button pauseBtn;

        private void LateUpdate()
        {
            DecodeAudioManager dam = DecodeAudioManager.m_instance;

            turnText.text = dam.currentTurn + "/" + dam.TurnNumber;

            if(dam.RemainingTime >= 0)
            {
                float normalizedTime = (float)dam.RemainingTime / dam.PlayTime;
                timerSlider.value = normalizedTime;
            }
            
        }

        public void DisableAll()
        {
            pauseBtn.interactable = false;
        }

        public void EnableAll()
        {
            pauseBtn.interactable=true;
        }
    }
}