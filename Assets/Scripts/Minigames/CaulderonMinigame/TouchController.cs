using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace CouldronMinigame
{
    public class TouchController : MonoBehaviour, IEndDragHandler, IDragHandler
    {
        [Header("References")]
        [SerializeField]
        private Image img_cauldron;

        [SerializeField] private RectTransform m_xTarget;
        [SerializeField] private RectTransform m_yTarget;

        [SerializeField] private CameraResolution m_cameraResolution;

        [SerializeField]
        private Image img_ladle;
        [SerializeField]
        SpeedController speedController;
        [Header("Parameters")]
        [SerializeField, Range(0.1f, 1.0f)]
        float paddingPercentage = 0.8f;
#if UNITY_EDITOR
        [SerializeField, Tooltip("Updates radiuses and center of ellipse when deforming screen, shouldn't be needed in release")]
        bool b_shouldUpdateDynamically = false;
#endif
        [SerializeField]
        private Image circularArrow;

        private Vector2 v2_ladleNewPosition;
        private float deltaFadeValue = 0.1f;


        private float f_xLimit, f_yLimit;
        private float theta,oldTheta;
        private Vector2 v2_cauldronCenter;
        private int thetaError=0;
        private int thetaRight = 0;
        private Vector3 m_oldPos;

        private void Start()
        {
            Debug.Log("Scaling: " + img_cauldron.canvas.scaleFactor);
            m_oldPos = this.transform.position;

            CalculateLimits();

            StartCoroutine(fadeIn());
        }

        private IEnumerator fadeIn() {
            float i = 0;
            do
            {
                i += deltaFadeValue;
                Color tmpColor = circularArrow.color;
                tmpColor.a = i;
                circularArrow.color = tmpColor;
                yield return new WaitForSeconds(deltaFadeValue/2);
            } while (i <= 1);
        }

        private IEnumerator fadeOut()
        {
            float i = 1;
            do
            {
                i -= deltaFadeValue;
                Color tmpColor = circularArrow.color;
                tmpColor.a = i;
                circularArrow.color = tmpColor;
                yield return new WaitForSeconds(deltaFadeValue/2);
            } while (i >= 0);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            speedController.setSpeed(0);
        }

        public void OnDrag(PointerEventData eventData)
        { CauldronGameManager gameManager = CauldronGameManager.Instance;
            if (gameManager != null && !gameManager.GameFinished) {
                Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
                Vector2 distanceFromCenter = TouchPosition - v2_cauldronCenter;
                oldTheta = theta;
                theta = Mathf.Atan2(distanceFromCenter.y, distanceFromCenter.x);
                //Debug.LogWarning("THETA: " + theta);

                float x = Mathf.Cos(theta);
                float y = Mathf.Sin(theta);

                Vector2 distanceVector = new Vector2(f_xLimit * y, f_yLimit * x);
                float k = (f_xLimit * f_yLimit) / distanceVector.magnitude;


                v2_ladleNewPosition = k * (new Vector2(x, y));

                Vector3 oldPos = this.transform.position;

                this.transform.position = v2_cauldronCenter + v2_ladleNewPosition;
                Debug.DrawRay(v2_cauldronCenter, v2_cauldronCenter + v2_ladleNewPosition, Color.red);
            }
        }

        private void Update()
        {


            if (isClockwiseMovement(oldTheta, theta) && thetaRight > 3)
            {
                float deltaMovement = (this.transform.position - m_oldPos).magnitude;

                speedController.setSpeed(deltaMovement);
                if (circularArrow.color.a >= 1)
                {
                    StartCoroutine(fadeOut());
                }
            }
            else if (thetaError > 3)
            {


                speedController.setSpeed(0);
                if (circularArrow.color.a <= 0)
                {
                    StartCoroutine(fadeIn());
                }
            }

            m_oldPos = this.transform.position;
        }

        private bool isClockwiseMovement(float oldTheta, float theta) {
            if (oldTheta >= 0)
            {
                if (oldTheta < theta)
                {
                    thetaRight = 0;
                    thetaError++;
                    return false;
                }
            }
            else {
                if (oldTheta < theta && theta < 2.0f) {
                    thetaRight = 0;
                    thetaError++;
                    return false;
                }
            }
            thetaError = 0;
            thetaRight++;
            return true;
        }

        private void CalculateLimits()
        {
            v2_cauldronCenter = img_cauldron.transform.position;

            f_xLimit = Vector2.Distance(v2_cauldronCenter, m_xTarget.transform.position);
            f_yLimit = Vector2.Distance(v2_cauldronCenter, m_yTarget.transform.position);

            f_xLimit *= paddingPercentage;
            f_yLimit *= paddingPercentage;
        }

#if UNITY_EDITOR
        private void LateUpdate()
        {
            if (b_shouldUpdateDynamically)
            {
                CalculateLimits();
            }
        }
#endif
    }
}


