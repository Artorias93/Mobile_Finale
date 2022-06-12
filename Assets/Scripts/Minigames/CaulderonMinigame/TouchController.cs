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

        private Vector2 v2_ladleNewPosition;


        private float f_xLimit, f_yLimit;
        private Vector2 v2_cauldronCenter;
        private void Start()
        {
            f_xLimit = (img_cauldron.rectTransform.sizeDelta.x * img_cauldron.canvas.scaleFactor) / 2;
            f_yLimit = (img_cauldron.rectTransform.sizeDelta.y * img_cauldron.canvas.scaleFactor) / 2;
            f_xLimit *= paddingPercentage;
            f_yLimit *= paddingPercentage;

            v2_cauldronCenter = img_cauldron.rectTransform.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            speedController.setMouseDelta(0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 distanceFromCenter = eventData.position - v2_cauldronCenter;

            float theta = Mathf.Atan2(distanceFromCenter.y, distanceFromCenter.x);

            float x = Mathf.Cos(theta);
            float y = Mathf.Sin(theta);

            Vector2 distanceVector = new Vector2(f_xLimit * y, f_yLimit * x);
            float k = (f_xLimit * f_yLimit) / distanceVector.magnitude;


            v2_ladleNewPosition = k * (new Vector2(x, y));
            v2_ladleNewPosition += v2_cauldronCenter;
            this.transform.position = v2_ladleNewPosition;
            speedController.setMouseDelta(eventData.delta.magnitude);
        }
    }
}


