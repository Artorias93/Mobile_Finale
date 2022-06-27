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
        
        [SerializeField, Tooltip("Lower: more responsive, Higher: less responsive")] private float f_speedCoefficient=0.4f;
        [SerializeField, Tooltip("Regulates how fast the slider increases"), Range(0.1f, 1f)] private float f_mouseAcceleration = 1f;
        [SerializeField, Tooltip("Regulates how fast the slider increases"), Range(0.1f, 5f)] private float f_maxSpeed = 2f; 

        private float m_speed;

        [Header("References")]
        [SerializeField] private Image m_imgSlow;
        [SerializeField] private Image m_imgFast;
        [SerializeField] private Image m_imgRegular;
        [SerializeField] private Image m_imgTooSlow;
        [SerializeField] private Image m_imgTooFast;
        [SerializeField]
        private Image m_insideCauldron;

        [SerializeField]
        private Slider m_speedIndicator;

        [SerializeField]
        private float m_trasparentAlpha=0.2f;
        private float m_visibleAlpha = 1f;

        private Color m_colorFast;
        private Color m_colorSlow;
        private Color m_colorRegular;

        private Gradient gradient;
        private GradientColorKey[] colorKey;
        private GradientAlphaKey[] alphaKey;
        


        private float timeSeconds;

        //usare enum per velocità
        public enum Speed { NOT_MOVING=0,SLOW=1, REGULAR=2, FAST=3,TOO_FAST=4 };

        [SerializeField] private float m_slowMinimumMovementSpeed = 0.05f;
        [SerializeField] private float m_regularMinimumMovementSpeed = 0.35f;
        [SerializeField] private float m_fastMinimumMovementSpeed = 0.65f;
        [SerializeField] private float m_tooFastMinimumMovementSpeed = 0.95f;



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

        private void SliderDynamicSize()
        {
            float AvailableSize = m_speedIndicator.GetComponent<RectTransform>().rect.width;

            float tooSlowWidth = AvailableSize * m_slowMinimumMovementSpeed;
            float tooFastWidth = AvailableSize * (1 - m_tooFastMinimumMovementSpeed);

            float slowWidth = AvailableSize * (m_regularMinimumMovementSpeed - m_slowMinimumMovementSpeed);
            float mediumWidth = AvailableSize * (m_fastMinimumMovementSpeed - m_regularMinimumMovementSpeed);
            float fastWidth = AvailableSize * (m_tooFastMinimumMovementSpeed - m_fastMinimumMovementSpeed);

            m_imgTooSlow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooSlowWidth);
            m_imgTooFast.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooFastWidth);

            m_imgSlow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slowWidth);
            m_imgRegular.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mediumWidth);
            m_imgFast.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fastWidth);
        }

        private void setGradientKeys() {
            gradient = new Gradient();
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.white;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.red;
            colorKey[1].time = 1.0f;
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
        }

        public void BeginPlay()
        {
            timeSeconds = gameManager.SelectedParameters.changeSecondsSpeedController;
            timeInError = gameManager.SelectedParameters.maxTimeInError;

            SliderDynamicSize();
            m_colorSlow = m_imgSlow.color;
            m_colorRegular = m_imgRegular.color;
            m_colorFast = m_imgFast.color;
            setGradientKeys();

            m_speed = 0;

            StartCoroutine(ChangeSpeed());
        }

        private void LateUpdate()
        {
            if (CauldronGameManager.Instance.GameFinished) return;

            float currentSliderValue = m_speedIndicator.value;
            float currentNormalizedSpeed = m_speed / f_maxSpeed;

            float deltaIndicator = Mathf.Lerp(currentSliderValue, currentNormalizedSpeed, Time.deltaTime * f_mouseAcceleration);
            deltaIndicator = Mathf.Clamp(deltaIndicator, 0.0f, 1.0f);
            m_speedIndicator.value = deltaIndicator;


            CheckSpeed();

            if (isInError) {
                timeInError -= Time.deltaTime;

                float percentageError = timeInError / gameManager.SelectedParameters.maxTimeInError;
                percentageError = 1 - percentageError;
                m_insideCauldron.color=gradient.Evaluate(percentageError);
            }

            if (timeInError <= 0)
                EndGame();

           // Debug.Log(timeInError);
        }
        public void setSpeed(float magnitude)
        {
            m_speed = magnitude;
            m_speed /= f_speedCoefficient;
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
            if (m_speedIndicator.value > m_tooFastMinimumMovementSpeed) {
                currentSpeed = Speed.TOO_FAST;
            } else if (m_speedIndicator.value <= m_tooFastMinimumMovementSpeed && m_speedIndicator.value > m_fastMinimumMovementSpeed) {
                currentSpeed = Speed.FAST;
            } else if (m_speedIndicator.value <= m_fastMinimumMovementSpeed && m_speedIndicator.value > m_regularMinimumMovementSpeed) {
                currentSpeed = Speed.REGULAR;
            } else if (m_speedIndicator.value<= m_regularMinimumMovementSpeed && m_speedIndicator.value>m_slowMinimumMovementSpeed) {
                currentSpeed = Speed.SLOW;
            } else {
                currentSpeed = Speed.NOT_MOVING;
            }

            if (randomSpeedIndex != currentSpeed)
                isInError = true;
            else
                isInError = false;

        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            SliderDynamicSize();
        }

#endif

    }
}
