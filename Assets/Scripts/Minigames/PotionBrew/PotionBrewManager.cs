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
        private int i_totalShakes = 6;
        [SerializeField]
        private int i_maxStepsCount = 5;

        public enum STEPS
        {
            SELECT,
            TILT,
            SHAKE
        }

        
        private STEPS e_currentStep;
        private GameObject m_currentTilting;
        private List<GameObject> selectablePotions;
        private GameObject m_potionToSelect;
        private GameObject m_shaKePotion;
        private ShakeController m_shaKePotionController;
        private float i_currentTiltSeconds=0;
        private Text m_stepText;

        private void Start()
        {
            selectablePotions = m_potionUIManager.Initialize(i_selectablePotions);
            m_shaKePotion = m_potionUIManager.m_shakePotion;
            m_shaKePotionController = m_shaKePotion.transform.GetChild(0).gameObject.GetComponent<ShakeController>();
            m_stepText = m_potionUIManager.m_stepText;
            for (int i = 0; i<selectablePotions.Count;i++) {
                GameObject elem = selectablePotions[i];
                elem.GetComponent<Button>().onClick.AddListener(() => { CheckAndSelectPotion(elem.gameObject); });
            }
            setSelectStep();


        }

        private void changeStep() {
            if (e_currentStep.Equals(STEPS.SELECT)) {
                e_currentStep = STEPS.TILT;
            }else if (e_currentStep.Equals(STEPS.TILT)) {
                if (Random.Range(0, 2) == 0)
                {
                    setShakeState();
                }
                else
                {
                    setSelectStep();
                }
            }else if (e_currentStep.Equals(STEPS.SHAKE))
            {
                setSelectStep();
            }

        }
        private void setSelectStep() {
            e_currentStep = STEPS.SELECT;
            m_stepText.text = "SELECT";
            m_potionToSelect = selectablePotions[Random.Range(0, selectablePotions.Count)];
            m_shaKePotion.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = m_potionToSelect.GetComponent<Image>().color;
        }
        private void setShakeState() {
            e_currentStep = STEPS.SHAKE;
            m_stepText.text = "SHAKE";
            m_shaKePotionController.b_startShake = true;
        }

        private void Update()
        {

            if (e_currentStep.Equals(STEPS.TILT))
            {
                CheckTiltState();
            }
            else if (e_currentStep.Equals(STEPS.SHAKE)) {             
                CheckShakeState();
            }
        }


        private void CheckShakeState() {
            if (m_shaKePotionController.i_ShakeCount >= i_totalShakes) {
                m_shaKePotionController.ResetShakeCount();
                m_shaKePotionController.b_startShake = false;
                changeStep();
            }
        }

        private void CheckTiltState() {
            if (i_currentTiltSeconds >= i_secondsToTilt)
            {
                m_currentTilting = null;
                m_potionUIManager.RemoveTiltable();
                i_currentTiltSeconds = 0;
                changeStep();
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
            if (m_currentTilting == null && e_currentStep.Equals(STEPS.SELECT) && m_potionToSelect.Equals(tiltPotion)) {
                m_currentTilting = m_potionUIManager.AddTiltable(tiltPotion);
                changeStep();
                m_stepText.text = "TILT";
            }
        }
    }
}