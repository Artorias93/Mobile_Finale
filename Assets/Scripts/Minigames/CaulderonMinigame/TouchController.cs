using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace CouldronMinigame
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField]
        private Image img_cauldron;
        [SerializeField]
        private Image img_ladle;
        [SerializeField]
        SpeedController speedController;
        [SerializeField, Range(0.1f, 1.0f)]
        float paddingPercentage = 0.8f;

        private Vector2 v2_ladleNewPosition;

        
        
        Vector2 m_oldMousePosition = Vector2.zero;

        private float f_xLimit, f_yLimit;
        private Vector2 v2_cauldronCenter;
        private void Awake()
        {
            f_xLimit = img_cauldron.rectTransform.rect.width/2;
            f_yLimit = img_cauldron.rectTransform.rect.height/2;
            f_xLimit *= paddingPercentage;
            f_yLimit *= paddingPercentage;

            v2_cauldronCenter = Camera.main.WorldToScreenPoint(img_cauldron.rectTransform.rect.center);

        }

        private void Update()
        {
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector2 distanceFromCenter= (touch.position - v2_cauldronCenter).normalized;
                    v2_ladleNewPosition = v2_cauldronCenter + new Vector2(distanceFromCenter.x*f_xLimit, distanceFromCenter.y*f_yLimit);
                    img_ladle.rectTransform.position = v2_ladleNewPosition;

                    speedController.setMouseDelta(touch.deltaPosition.magnitude);
                    return; //Only first touch will count
                }
            }
            speedController.setMouseDelta(0);
            
        }
    }
}


