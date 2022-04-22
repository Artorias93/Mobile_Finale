using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PotionBrewMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_selectablePotions;
        public int i_secondsToTilt;
        public int i_totalShakes;
        public int i_maxStepsCount;
        public int i_maxOverflowSeconds; 
    }

    public class PotionBrewManager : AMinigameManager
    {
        //013835
        [SerializeField]
        private PotionUiManager m_potionUIManager;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        private GameParameters m_gameParameters;


        public enum STEPS
        {
            SELECT,
            TILT,
            SHAKE,
            FINISH
        }

        
        private STEPS e_currentStep;
        private GameObject m_currentTiltingPotion;
        private List<GameObject> selectablePotions;
        private GameObject m_potionToSelect;
        private GameObject m_shaKePotion;
        private ShakeController m_shaKePotionController;
        private float f_currentTiltSeconds=0;
        private float f_currentTiltOverflowSeconds = 0;
        private int i_currentStepCount = 0;
        private Text m_stepText;
        private Text m_flowText;

        private void Start()
        {
            //DEBUG
            BeginMinigame(Difficulty.HARD);

        }
        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;

            if (m_difficulty == Difficulty.EASY)
                m_gameParameters = gp_easy;
            else if (m_difficulty == Difficulty.MEDIUM)
                m_gameParameters = gp_medium;
            else if (m_difficulty == Difficulty.HARD)
                m_gameParameters = gp_hard;
            InitializeRound();
        }

        private void InitializeRound()
        {
            selectablePotions = m_potionUIManager.Initialize(m_gameParameters.i_selectablePotions);
            m_shaKePotion = m_potionUIManager.m_shakePotion;
            m_shaKePotionController = m_shaKePotion.transform.GetChild(0).gameObject.GetComponent<ShakeController>();
            m_stepText = m_potionUIManager.m_stepText;
            m_flowText = m_potionUIManager.m_flowText;
            m_flowText.text = "";
            for (int i = 0; i < selectablePotions.Count; i++)
            {
                GameObject elem = selectablePotions[i];
                elem.GetComponent<Button>().onClick.AddListener(() => { CheckAndSelectPotion(elem.gameObject); });
            }
            setSelectStep();
        }
        private void changeStep() {
            i_currentStepCount++;
            if (i_currentStepCount >= m_gameParameters.i_maxStepsCount) {
                setFinishState(true);
            }
            if (e_currentStep.Equals(STEPS.SELECT)) {
                e_currentStep = STEPS.TILT;
            }else if (e_currentStep.Equals(STEPS.TILT)) {
                m_flowText.text = "";
                if (m_gameParameters.i_maxStepsCount - i_currentStepCount == 1)
                {
                    setShakeState();
                }
                else {
                    if (Random.Range(0, 2) == 0)
                    {
                        setShakeState();
                    }
                    else
                    {
                        setSelectStep();
                    }
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

        private void setFinishState(bool b_win) {
            e_currentStep = STEPS.FINISH;
            if (b_win)
            {
                m_stepText.text = "YOU WIN";
            }
            else {
                m_stepText.text = "YOU LOST";
            }
            
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
            if (m_shaKePotionController.i_ShakeCount >= m_gameParameters.i_totalShakes) {
                m_shaKePotionController.ResetShakeCount();
                m_shaKePotionController.b_startShake = false;
                changeStep();
            }
        }

        private void CheckTiltState() {
            if (f_currentTiltSeconds >= m_gameParameters.i_secondsToTilt)
            {
                m_currentTiltingPotion = null;
                m_potionUIManager.RemoveTiltable();
                f_currentTiltSeconds = 0;
                f_currentTiltOverflowSeconds = 0;
                changeStep();
            } else if (f_currentTiltOverflowSeconds>= m_gameParameters.i_maxOverflowSeconds) {
                setFinishState(false);
            }
            else if (m_currentTiltingPotion.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.SLOW))
            {
                m_flowText.text = "SLOW";
                f_currentTiltSeconds += (Time.deltaTime / 2);
            }
            else if (m_currentTiltingPotion.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.MEDIUM))
            {
                m_flowText.text = "MEDIUM";
                f_currentTiltSeconds += Time.deltaTime;
            }
            else if (m_currentTiltingPotion.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.FAST))
            {
                m_flowText.text = "FAST";
                f_currentTiltSeconds += (Time.deltaTime * 2);
            }
            else if (m_currentTiltingPotion.GetComponent<TiltController>().e_FlowState.Equals(TiltController.FLOW_SPEED.OVERFLOW))
            {
                m_flowText.text = "OVERFLOW";
                f_currentTiltOverflowSeconds+= Time.deltaTime;
            }
            else {
                m_flowText.text = "NO";
            }
        }
        private void CheckAndSelectPotion(GameObject tiltPotion) {
            if (m_currentTiltingPotion == null && e_currentStep.Equals(STEPS.SELECT) && m_potionToSelect.Equals(tiltPotion)) {
                m_currentTiltingPotion = m_potionUIManager.AddTiltable(tiltPotion);
                changeStep();
                m_stepText.text = "TILT";
            }
        }
        public override void EndMinigame()
        {
            throw new System.NotImplementedException();
        }
    }
}