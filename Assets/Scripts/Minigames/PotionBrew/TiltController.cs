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
        private float f_slowAngle = 20;
        [SerializeField]
        private float f_mediumAngle = 45;
        [SerializeField]
        private float f_fastAngle = 70;
        [SerializeField]
        private float f_maxAngle = 90;

        public FLOW_SPEED e_FlowState {
            get { return e_flowState; }
        }
        public enum FLOW_SPEED { 
            NO,
            SLOW,
            MEDIUM,
            FAST
        }

        private Gyroscope m_Gyro;
        private FLOW_SPEED e_flowState = FLOW_SPEED.NO;


        void Start()
        {
            //Set up and enable the gyroscope (check your device has one)
            m_Gyro = Input.gyro;
            m_Gyro.enabled = true;
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

                    if (zAngle > f_minAngle && zAngle < f_slowAngle)
                    {
                        e_flowState = FLOW_SPEED.NO;
                    }
                    else if (zAngle > f_slowAngle && zAngle < f_mediumAngle)
                    {
                        e_flowState = FLOW_SPEED.SLOW;
                    }
                    else if (zAngle > f_mediumAngle && zAngle < f_fastAngle)
                    {
                        e_flowState = FLOW_SPEED.MEDIUM;
                    }
                    else if (zAngle > f_fastAngle && zAngle < f_maxAngle)
                    {
                        e_flowState = FLOW_SPEED.FAST;
                    }

                }
                else {
                    transform.eulerAngles = previousEulerAngles;
                }
            }
        }
    }
}
