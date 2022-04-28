using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace CouldronMinigame
{
    public class TouchController : MonoBehaviour
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

        private Vector2 v2_ladleNewPosition;


        private float f_xLimit, f_yLimit;
        private Vector2 v2_cauldronCenter;
        private void Start()
        {
            f_xLimit = (img_cauldron.rectTransform.sizeDelta.x * img_cauldron.canvas.scaleFactor) / 2;
            f_yLimit = (img_cauldron.rectTransform.sizeDelta.y * img_cauldron.canvas.scaleFactor) / 2;
            f_xLimit *= paddingPercentage;
            f_yLimit *= paddingPercentage;

            v2_cauldronCenter = Camera.main.WorldToScreenPoint(img_cauldron.rectTransform.anchoredPosition);
        }

        private void Update()
        {
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {

                    //This math could possibly be simplified, but is held like this for readibility
                    //Computes the projection of a point on an ellipse
                    Vector2 distanceFromCenter = touch.position - v2_cauldronCenter;
                    float theta = Mathf.Atan2(distanceFromCenter.y, distanceFromCenter.x);

                    float x = Mathf.Cos(theta);
                    float y = Mathf.Sin(theta);

                    Vector2 distanceVector = new Vector2(f_xLimit * y, f_yLimit * x);
                    float k = (f_xLimit * f_yLimit) / distanceVector.magnitude;


                    v2_ladleNewPosition = k*(new Vector2(x, y));
                    v2_ladleNewPosition += v2_cauldronCenter;

                    img_ladle.rectTransform.position = v2_ladleNewPosition;

                    Debug.DrawLine(v2_cauldronCenter, touch.position, Color.green);
                    Debug.DrawLine(v2_cauldronCenter, v2_ladleNewPosition, Color.red);

                    speedController.setMouseDelta(touch.deltaPosition.magnitude);
                    return; //Only first touch will count
                }
            }
            speedController.setMouseDelta(0);
        }


        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (b_shouldUpdateDynamically)
            {
                f_xLimit = (img_cauldron.rectTransform.sizeDelta.x * img_cauldron.canvas.scaleFactor) / 2;
                f_yLimit = (img_cauldron.rectTransform.sizeDelta.y * img_cauldron.canvas.scaleFactor) / 2;
                f_xLimit *= paddingPercentage;
                f_yLimit *= paddingPercentage;

                v2_cauldronCenter = Camera.main.WorldToScreenPoint(img_cauldron.rectTransform.anchoredPosition);
            }
#endif
        }

    }
}


