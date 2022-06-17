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
        private void Start()
        {
            f_xLimit = (img_cauldron.rectTransform.sizeDelta.x * img_cauldron.canvas.scaleFactor) / 2;
            f_yLimit = (img_cauldron.rectTransform.sizeDelta.y * img_cauldron.canvas.scaleFactor) / 2;
            f_xLimit *= paddingPercentage;
            f_yLimit *= paddingPercentage;

            v2_cauldronCenter = img_cauldron.rectTransform.position;
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
            speedController.setMouseDelta(0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 distanceFromCenter = eventData.position - v2_cauldronCenter;
            oldTheta = theta;
            theta = Mathf.Atan2(distanceFromCenter.y, distanceFromCenter.x);
            //Debug.LogWarning("THETA: " + theta);
            
            float x = Mathf.Cos(theta);
            float y = Mathf.Sin(theta);

            Vector2 distanceVector = new Vector2(f_xLimit * y, f_yLimit * x);
            float k = (f_xLimit * f_yLimit) / distanceVector.magnitude;


            v2_ladleNewPosition = k * (new Vector2(x, y));
            v2_ladleNewPosition += v2_cauldronCenter;
            this.transform.position = v2_ladleNewPosition;
            if (isClockwiseMovement(oldTheta, theta) && thetaRight>3)
            {

                speedController.setMouseDelta(eventData.delta.magnitude);
                if (circularArrow.color.a>=1) {
                    StartCoroutine(fadeOut());
                }
            }
            else if(thetaError>3){

                
                speedController.setMouseDelta(0);
                if (circularArrow.color.a<=0)
                {
                    StartCoroutine(fadeIn());
                }
            }
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
    }
}


