using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace OuijaBoardMinigame
{
    public class OuijaBoardUIManager : MonoBehaviour
    {
        [Header("References")]
        public Text clearedWords;
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Button pauseBtn;
        public GameObject target;
        public GameObject results;

        private void LateUpdate()
        {
            OuijaBoardManager obm = OuijaBoardManager.m_instance;

            if(obm.RemainingTime >= 0)
            {
                float normalizedTime = (float)obm.RemainingTime / obm.PlayTime;
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