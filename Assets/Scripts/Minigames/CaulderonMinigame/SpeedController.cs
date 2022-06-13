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

        [Header("Input Parameters")]
        
        [SerializeField] private float m_maxSpeed=1000;
        [SerializeField, Tooltip("Regulates how fast the slider decreases"), Range(-1.0f, -0.1f)] private float f_mouseDeceleration = -0.8f;
        [SerializeField, Tooltip("Regulates how fast the slider increases"), Range(0.1f, 1f)] private float f_mouseAcceleration = 1f;

        private float m_speed;
        private float mouseDelta;

        [Header("References")]
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
        
        private float timeSeconds;

        //usare enum per velocità
        public enum Speed { NOT_MOVING=0,SLOW=1, REGULAR=2, FAST=3,TOO_FAST=4 };
       

        private float m_minimumRegularNormalizedSpeed = 0.25f;
        private float m_mediumNormalizedSpeed = 0.5f;
        private float m_maximumRegularNormalizedSpeed = 0.75f;



        private Speed randomSpeedIndex;
        private Speed currentSpeed;
        

        [SerializeField] private CauldronGameManager gameManager;

        private float timeInError = float.MaxValue;
        private bool isInError = false;

        public float TimeinError
        {

            get { return timeInError; }
            set { timeInError = value; }
        }
        void Start()
        {
            m_colorSlow = m_imgSlow.color;
            m_colorRegular = m_imgRegular.color;
            m_colorFast = m_imgFast.color;


        }

        public void BeginPlay()
        {
            timeSeconds = gameManager.GameParameters.changeSecondsSpeedController;
            timeInError = gameManager.GameParameters.maxTimeInError;

            StartCoroutine(ChangeSpeed());
        }

        private void Update()
        {
            if (CauldronGameManager.Instance.GameFinished) return;

            float speedDelta = 2*((mouseDelta - m_maxSpeed/2) / m_maxSpeed);
            speedDelta = Mathf.Clamp(speedDelta, f_mouseDeceleration, f_mouseAcceleration);
            m_speed += speedDelta*Time.deltaTime;
            m_speed = Mathf.Clamp(m_speed, 0, 1);

            CheckSpeed();

            if (isInError)
                timeInError -= Time.deltaTime;
            
            if (timeInError <= 0)
                EndGame();

           // Debug.Log(timeInError);
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
                Debug.Log("changing speed...");
                RandomSpeed();
                yield return new WaitForSeconds(timeSeconds);
            }
        }


        private void RandomSpeed()
        {
            Debug.Log("setting new speed...");
            
            randomSpeedIndex = (Speed)Random.Range(1, 3);

            if (randomSpeedIndex == Speed.REGULAR)
            {
                //modifica colore poi metti colore in immagine
                m_colorRegular.a = m_visibleAlpha;
                m_imgRegular.color = m_colorRegular;

                m_colorFast.a = m_trasparentAlpha;
                m_imgFast.color = m_colorFast;

                m_colorSlow.a = m_trasparentAlpha;
                m_imgSlow.color = m_colorSlow;

            }
            else if (randomSpeedIndex == Speed.SLOW)
            {
                m_colorSlow.a = m_visibleAlpha;
                m_imgSlow.color = m_colorSlow;

                m_colorFast.a = m_trasparentAlpha;
                m_imgFast.color = m_colorFast;

                m_colorRegular.a = m_trasparentAlpha;
                m_imgRegular.color = m_colorRegular;

            }
            else if (randomSpeedIndex == Speed.FAST)
            {
                m_colorFast.a = m_visibleAlpha;
                m_imgFast.color = m_colorFast;

                m_colorSlow.a = m_trasparentAlpha;
                m_imgSlow.color = m_colorSlow;

                m_colorRegular.a = m_trasparentAlpha;
                m_imgRegular.color = m_colorRegular;

            }
        }

        
        private void EndGame()
        {
            gameManager.LoseMinigame();
        }

        private void CheckSpeed()
        {
            if (m_speedIndicator.value > m_maximumRegularNormalizedSpeed) {
                currentSpeed = Speed.TOO_FAST;
            } else if (m_speedIndicator.value <= m_maximumRegularNormalizedSpeed && m_speedIndicator.value > m_mediumNormalizedSpeed) {
                currentSpeed = Speed.FAST;
            } else if (m_speedIndicator.value <= m_mediumNormalizedSpeed && m_speedIndicator.value > m_minimumRegularNormalizedSpeed) {
                currentSpeed = Speed.REGULAR;
            } else if (m_speedIndicator.value<=m_minimumRegularNormalizedSpeed && m_speedIndicator.value>0) {
                currentSpeed = Speed.SLOW;
            } else {
                currentSpeed = Speed.NOT_MOVING;
            }

            if (randomSpeedIndex != currentSpeed)
                isInError = true;
            else
                isInError = false;

        }


    }
}
