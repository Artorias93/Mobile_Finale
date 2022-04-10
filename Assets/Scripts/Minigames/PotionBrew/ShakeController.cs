using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionBrewMinigame
{
    public class ShakeController : MonoBehaviour
    {
        public float F_ShakeTreshOld = 4f;

        private Rigidbody2D m_rigidBody;
        private float f_magnitude = 0f;
        private Vector3 m_acceleration = Vector3.zero;
        //Public for debug
        public bool b_startShake = false;


        // Start is called before the first frame update
        void Start()
        {
            m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        }
        public void InitShakeController()
        {
            b_startShake = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (b_startShake)
            {
                f_magnitude = Input.acceleration.magnitude;
                m_acceleration = Input.acceleration;
            }
        }
        private void FixedUpdate()
        {
            if (f_magnitude >= F_ShakeTreshOld)
            {
                shake(m_acceleration);
            }

        }

        private void shake(Vector3 deviceAcceleration)
        {
            deviceAcceleration.y = Mathf.Pow(deviceAcceleration.z, 2);
            m_rigidBody.AddForce(deviceAcceleration, ForceMode2D.Impulse);
        }
    }
}