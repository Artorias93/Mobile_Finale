using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PotionBrewMinigame
{
    [System.Serializable]
    public struct GameParameters
    {
        public float f_gameTime;
        public int i_selectablePotions;
        public int i_secondsToTilt;
        public int i_totalShakes;
        public int i_maxStepsCount;
        public int i_maxOverflowSeconds; 
    }

    public class PotionBrewManager : AMinigameManager
    {
        
        [SerializeField]
        private PotionUiManager m_potionUIManager;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;
        [SerializeField] private GeneralUIManager generalUIManager;
        private GameParameters m_gameParameters;
        public static PotionBrewManager Instance;
        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }
        public int CurrentStepCount {
            get { return i_currentStepCount; }
        }

        public enum STEPS
        {
            NOT_STARTED,
            SELECT,
            TILT,
            SHAKE,
            FINISH
        }
        public GameParameters GameParameters
        {
            get { return m_gameParameters; }
        }

        public STEPS CurrentStep {
            get { return e_currentStep; }
        }
        public bool GameWon {
            get { return b_gameWon; }
        }

        private STEPS e_currentStep = STEPS.NOT_STARTED;
        private TiltController m_currentTiltingPotionController;
        private List<GameObject> selectablePotions;
        private GameObject m_potionToSelect;
        private GameObject m_shaKePotion;
        private ShakeController m_shaKePotionController;
        private float f_currentTiltSeconds=0;
        private float f_currentTiltOverflowSeconds = 0;
        private int i_currentStepCount = 0;
        private Text m_stepText;
        private Text m_flowText;
        private Timer m_gameTimer;
        private bool b_gameWon;

        private void Awake()
        {
            Instance = this;
            b_gameWon = false;
        }
        private void Start()
        {
            //Remove comment for scene debug
            //BeginMinigame(Difficulty.MEDIUM);
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
            generalUIManager.EnableAll();
            InitializeRound();
        }

        private void InitializeRound()
        {
            selectablePotions = m_potionUIManager.Initialize(m_gameParameters.i_selectablePotions);
            m_shaKePotion = m_potionUIManager.m_shakePotion.transform.GetChild(0).gameObject;
            m_shaKePotionController = m_shaKePotion.GetComponent<ShakeController>();
            m_stepText = m_potionUIManager.m_stepText;
            m_flowText = m_potionUIManager.m_flowText;
            m_flowText.text = "";
            for (int i = 0; i < selectablePotions.Count; i++)
            {
                GameObject elem = selectablePotions[i];
                elem.GetComponent<Button>().onClick.AddListener(() => { CheckAndSelectPotion(elem.gameObject); });
            }
            m_gameTimer = Timer.Create(m_gameParameters.f_gameTime, EndMinigame, destroyOnFinish: true, name: "Minigame Timer");
            m_gameTimer.StartTimer();
            setSelectStep();
        }
        private void changeStep() {
            i_currentStepCount++;
            if (i_currentStepCount >= m_gameParameters.i_maxStepsCount) {
                setFinishStep(true);
            }
            if (e_currentStep.Equals(STEPS.SELECT)) {
                e_currentStep = STEPS.TILT;
            }else if (e_currentStep.Equals(STEPS.TILT)) {
                m_flowText.text = "";
                if (m_gameParameters.i_maxStepsCount - i_currentStepCount == 1)
                {
                    setShakeStep();
                }
                else {
                    if (Random.Range(0, 2) == 0)
                    {
                        setShakeStep();
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
            m_potionUIManager.setSelectImage(m_potionToSelect.transform.GetChild(0).GetComponent<Image>().color); 
        }
        private void setShakeStep() {
            e_currentStep = STEPS.SHAKE;
            m_stepText.text = "SHAKE";
            m_potionUIManager.RemoveSelectImage();
            m_shaKePotionController.b_startShake = true;
        }

        private void setFinishStep(bool b_win) {
            e_currentStep = STEPS.FINISH;
            m_potionUIManager.RemoveSelectImage();
            m_shaKePotionController.b_startShake = false;
            if (m_currentTiltingPotionController!= null)
            {
                m_currentTiltingPotionController = null;
                m_potionUIManager.RemoveTiltable();
            }
            b_gameWon = b_win;
            m_flowText.text = "";
            m_stepText.text = "";
            m_gameTimer.StopTimer();
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
                m_currentTiltingPotionController = null;
                m_potionUIManager.RemoveTiltable();
                f_currentTiltSeconds = 0;
                f_currentTiltOverflowSeconds = 0;
                m_shaKePotionController.setBaseSprite();
                changeStep();
            }
            else if (f_currentTiltOverflowSeconds >= m_gameParameters.i_maxOverflowSeconds)
            {
                setFinishStep(false);
            }
            else {
                if (!m_currentTiltingPotionController.e_FlowState.Equals(TiltController.FLOW_SPEED.OVERFLOW))
                {
                    m_shaKePotionController.setBaseSprite();
                    if (m_currentTiltingPotionController.e_FlowState.Equals(TiltController.FLOW_SPEED.SLOW))
                    {
                        m_flowText.text = "SLOW";
                        f_currentTiltSeconds += (Time.deltaTime / 2);
                    }
                    else if (m_currentTiltingPotionController.e_FlowState.Equals(TiltController.FLOW_SPEED.MEDIUM))
                    {
                        m_flowText.text = "MEDIUM";
                        f_currentTiltSeconds += Time.deltaTime;
                    }
                    else if (m_currentTiltingPotionController.e_FlowState.Equals(TiltController.FLOW_SPEED.FAST))
                    {
                        m_flowText.text = "FAST";
                        f_currentTiltSeconds += (Time.deltaTime * 2);
                    }
                    else
                    {
                        m_flowText.text = "NO";
                    }

                }
                else {
                    m_flowText.text = "OVERFLOW";
                    f_currentTiltOverflowSeconds += Time.deltaTime;
                    m_shaKePotionController.setOverFlowSprite();
                }

            }
        }
        private void CheckAndSelectPotion(GameObject tiltPotion) {
            if (m_currentTiltingPotionController == null && e_currentStep.Equals(STEPS.SELECT) && m_potionToSelect.Equals(tiltPotion)) {
                GameObject go = m_potionUIManager.AddTiltable(tiltPotion);
                m_currentTiltingPotionController =go.transform.GetChild(0).gameObject.GetComponent<TiltController>();
                m_potionUIManager.RemoveSelectImage();
                changeStep();
                m_stepText.text = "TILT";
            }
        }
        protected override void EndMinigame()
        {
            if (m_gameTimer != null && m_gameTimer.RemainingTime <= 0.0f) {
                setFinishStep(false);
                generalUIManager.DisableAll();
            }
        }

        public override void PauseGame()
        {
            base.PauseGame();
            AudioListener.pause = true;
        }

        public override void ResumeGame()
        {
            base.ResumeGame();
            AudioListener.pause = false;
        }
    }
}