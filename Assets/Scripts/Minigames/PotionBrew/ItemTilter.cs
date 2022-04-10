using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PotionBrewMinigame
{
    public class ItemTilter : MonoBehaviour
    {
        [SerializeField]
        public float f_minAngle = 0;
        [SerializeField]
        public float f_maxAngle = 90;
        [SerializeField]
        public float f_angleTollerance = 5;

        private Gyroscope m_Gyro;

        void Start()
        {
            //Set up and enable the gyroscope (check your device has one)
            m_Gyro = Input.gyro;
            m_Gyro.enabled = true;
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

                if (targetEulerAngles.z > f_minAngle && targetEulerAngles.z < f_maxAngle)
                {
                    transform.eulerAngles = targetEulerAngles;
                }
                else
                {
                    transform.eulerAngles = previousEulerAngles;
                }
            }
        }
    }
}
