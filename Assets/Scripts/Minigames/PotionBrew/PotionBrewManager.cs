using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PotionBrewMinigame
{
    public class PotionBrewManager : MonoBehaviour
    {
        //013835
        [SerializeField]
        private PotionUiManager m_potionUIManager;
        [SerializeField]
        private int i_selectablePotions = 4;
        [SerializeField]
        private int i_secondsToTilt = 5;
        [SerializeField]
        private int i_totalShakes = 3;

        public enum steps
        {
            SELECT,
            TILT,
            SHAKE
        }

        
        private steps currentStep;
        private GameObject m_currentTilting;
        private List<GameObject> selectablePotions;
        private GameObject m_potionToSelect;
        private GameObject m_shaKePotion;
        private float i_currentTiltSeconds=0;
        private int i_currentShakes;
        private void Start()
        {
            selectablePotions = m_potionUIManager.Initialize(i_selectablePotions);
            m_shaKePotion = m_potionUIManager.m_shakePotion;
            for (int i = 0; i<selectablePotions.Count;i++) {
                GameObject elem = selectablePotions[i];
                elem.GetComponent<Button>().onClick.AddListener(() => { CheckAndSelectPotion(elem.gameObject); });
            }
            setSelectStep();


        }
        private void setSelectStep() {
            currentStep = steps.SELECT;
            m_potionToSelect = selectablePotions[Random.Range(0, selectablePotions.Count)];
            m_shaKePotion.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = m_potionToSelect.GetComponent<Image>().color;
        }

        private void Update()
        {

            if (currentStep.Equals(steps.TILT))
            {
                CheckTiltState();
            }
        }


        private void CheckTiltState() {
            if (i_currentTiltSeconds >= i_secondsToTilt)
            {
                m_currentTilting = null;
                m_potionUIManager.RemoveTiltable();
                i_currentTiltSeconds = 0;
                setSelectStep();
                //Rimuovo il tilting item e cambio stato
            }
            else if (m_currentTilting.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.SLOW))
            {
                i_currentTiltSeconds += (Time.deltaTime / 2);
                //Cambiare Sprite
            }
            else if (m_currentTilting.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.MEDIUM))
            {
                i_currentTiltSeconds += Time.deltaTime;
                //Cambiare Sprite
            }
            else if (m_currentTilting.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.FAST))
            {
                i_currentTiltSeconds += (Time.deltaTime * 2);
                //Cambiare Sprite
            }
        }
        private void CheckAndSelectPotion(GameObject tiltPotion) {
            if (m_currentTilting == null && currentStep.Equals(steps.SELECT) && m_potionToSelect.Equals(tiltPotion)) {
                m_currentTilting = m_potionUIManager.AddTiltable(tiltPotion);
                currentStep = steps.TILT;
            }
        }
    }
}