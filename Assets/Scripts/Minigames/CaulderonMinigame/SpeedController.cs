using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CouldronMinigame
{
    public class SpeedController : MonoBehaviour
    {

        
        private float f_ladleCurrentSpeed;
        [SerializeField] private float m_maxSpeed=40000;


        private float m_speed;
        private float mouseDelta;
       

        [SerializeField]
        private Image m_imgSlow;
        [SerializeField]
        private Image m_imgFast;
        [SerializeField]
        private Image m_imgRegular;

        [SerializeField]
        private Slider m_speedIndicator;

        [SerializeField]
        private float m_trasparentAlpha=0.2f;
        private float m_visibleAlpha = 1f;

        private Color m_colorFast;
        
        private Color m_colorSlow;
        
        private Color m_colorRegular;
        
        public float startCheckTimeSeconds = 3f;

        //usare enum per velocità
        public enum Speed { SLOW=0, REGULAR=1, FAST=2 };

        private float m_minimumRegularNormalizedSpeed = 0.33f;
        private float m_maximumRegularNormalizedSpeed = 0.66f;
        
        private int randomSpeedIndex;
        

        void Start()
        {
            m_colorSlow = m_imgSlow.color;
            m_colorRegular = m_imgRegular.color;
            m_colorFast = m_imgFast.color;

            
            StartCoroutine(ChangeSpeed());
            
        }


        private void Update()
        {
            
            float speedDelta = 2*((mouseDelta - m_maxSpeed/2) / m_maxSpeed);
            speedDelta = Mathf.Clamp(speedDelta, -1, 1);
            m_speed += speedDelta*Time.deltaTime;
            m_speed = Mathf.Clamp(m_speed, 0, 1);

        }
        public void setMouseDelta(float magnitude)
        {
            mouseDelta = magnitude;
        }
        void LateUpdate()
        {
            
            m_speedIndicator.value = m_speed;
        }


        private IEnumerator ChangeSpeed()
        {
            //chiamo la random, dopo n secondi faccio il check della ladle e la velocità---se non è andato in gameover ripeto
            while (true)
            {
                Debug.Log("in corutine");
                RandomSpeed();
                yield return new WaitForSeconds(startCheckTimeSeconds);
                CheckSpeed();
            }
        }

        

        private void RandomSpeed()
        {
            Debug.Log("in random speed");
            
                randomSpeedIndex = Random.Range(0, 3);
            
            if (randomSpeedIndex == (int)Speed.REGULAR)
            {
                //modifica colore poi metti colore in immagine
                m_colorRegular.a = m_visibleAlpha;
                m_imgRegular.color = m_colorRegular;

                m_colorFast.a = m_trasparentAlpha;
                m_imgFast.color = m_colorFast;

                m_colorSlow.a = m_trasparentAlpha;
                m_imgSlow.color = m_colorSlow;

            }
            else if (randomSpeedIndex == (int)Speed.SLOW)
            {
                m_colorSlow.a = m_visibleAlpha;
                m_imgSlow.color = m_colorSlow;

                m_colorFast.a = m_trasparentAlpha;
                m_imgFast.color = m_colorFast;

                m_colorRegular.a = m_trasparentAlpha;
                m_imgRegular.color = m_colorRegular;

            }
            else if (randomSpeedIndex == (int)Speed.FAST)
            {
                m_colorFast.a = m_visibleAlpha;
                m_imgFast.color = m_colorFast;

                m_colorSlow.a = m_trasparentAlpha;
                m_imgSlow.color = m_colorSlow;

                m_colorRegular.a = m_trasparentAlpha;
                m_imgRegular.color = m_colorRegular;

            }
        }

     
        private void CheckSpeed()
        {
            //controlla se sei in gameover
           
            Debug.Log("in checkspeed");
            Debug.Log(f_ladleCurrentSpeed);
            if(f_ladleCurrentSpeed == 0)
            {
                //se stai fermo dopo tot tempo  gameover
                Debug.Log("gameover fermo");
            }
            else if(m_speedIndicator.value > 0 && m_speedIndicator.value <= m_minimumRegularNormalizedSpeed)
            {
                //sei in slow -> randomSpeedIndex = 0
                if (randomSpeedIndex != 0)
                {
                    //gameover
                    Debug.Log("gameover");
                }
            }
            else if (m_speedIndicator.value > 0 && m_speedIndicator.value >= m_minimumRegularNormalizedSpeed && m_speedIndicator.value <= m_maximumRegularNormalizedSpeed)
            {
                //sei in regular  -> randomSpeedIndex = 1
                if (randomSpeedIndex != 1)
                {
                    //gameover
                    Debug.Log("gameover");

                }
            }
            else if (m_speedIndicator.value > 0 && m_speedIndicator.value >= m_maximumRegularNormalizedSpeed)
            {
                //sei in fast  -> randomSpeedIndex = 2
                if (randomSpeedIndex != 2)
                {
                    //gameover
                    Debug.Log("gameover");

                }
            }




        }


    }
}
