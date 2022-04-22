using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace PotionBrewMinigame
{
    public class TiltController : MonoBehaviour
    {
        [SerializeField]
        private float f_minAngle = 0;
        [SerializeField]
        private float f_slowAngle = 57;//45
        [SerializeField]
        private float f_mediumAngle = 62;//60
        [SerializeField]
        private float f_fastAngle = 67;//65-70
        [SerializeField]
        private float f_overFlowAngle = 72;//70-75
        [SerializeField]
        private float f_maxAngle = 90;
        [SerializeField]
        private Sprite s_potionNoFlow;
        [SerializeField]
        private Sprite s_potionSlowFlow;
        [SerializeField]
        private Sprite s_potionMediumFlow;
        [SerializeField]
        private Sprite s_potionFastFlow;
        [SerializeField]
        private Sprite s_potionOverFlow;


        public FLOW_SPEED e_FlowState {
            get { return e_flowState; }
        }
        public enum FLOW_SPEED { 
            NO,
            SLOW,
            MEDIUM,
            FAST,
            OVERFLOW
        }

        private Gyroscope m_Gyro;
        private FLOW_SPEED e_flowState = FLOW_SPEED.NO;
        private SpriteRenderer m_spriteRender;

        void Start()
        {
            //Set up and enable the gyroscope (check your device has one)
            m_Gyro = Input.gyro;
            m_Gyro.enabled = true;
            m_spriteRender = GetComponent<SpriteRenderer>();
        }

        public void StartTilt()
        {
            m_Gyro.enabled = true;
        }
        public void StopTilt()
        {
            m_Gyro.enabled = false;
        }
        // Update is called once per frame
        void Update()
        {
            if (m_Gyro.enabled)
            {
                Vector3 previousEulerAngles = transform.eulerAngles;
                Vector3 gyroInput = Input.gyro.rotationRateUnbiased;

                Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
                targetEulerAngles.x = 0.0f;
                targetEulerAngles.y = 0.0f;
                float zAngle = targetEulerAngles.z;
                if (zAngle > f_minAngle && zAngle < f_maxAngle)
                {
                    transform.eulerAngles = targetEulerAngles;

                    if (zAngle > f_minAngle && zAngle < f_slowAngle && !e_flowState.Equals(FLOW_SPEED.NO))
                    {
                        e_flowState = FLOW_SPEED.NO;
                        m_spriteRender.sprite = s_potionNoFlow;
                    }
                    else if (zAngle > f_slowAngle && zAngle < f_mediumAngle && !e_flowState.Equals(FLOW_SPEED.SLOW) )
                    {
                        e_flowState = FLOW_SPEED.SLOW;
                        m_spriteRender.sprite = s_potionSlowFlow;
                    }
                    else if (zAngle > f_mediumAngle && zAngle < f_fastAngle && !e_flowState.Equals(FLOW_SPEED.MEDIUM))
                    {
                        e_flowState = FLOW_SPEED.MEDIUM;
                    }
                    else if (zAngle > f_fastAngle && zAngle < f_overFlowAngle && !e_flowState.Equals(FLOW_SPEED.FAST))
                    {
                        e_flowState = FLOW_SPEED.FAST;
                    }
                    else if (zAngle > f_overFlowAngle && zAngle < f_maxAngle && !e_flowState.Equals(FLOW_SPEED.OVERFLOW))
                    {
                        e_flowState = FLOW_SPEED.OVERFLOW;
                    }

                }
                else {
                    transform.eulerAngles = previousEulerAngles;
                }
            }
        }
    }
}
