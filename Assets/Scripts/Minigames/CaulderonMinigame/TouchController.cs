using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace CouldronMinigame
{
    public class TouchController : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField]
        private Image img_cauldron;
        [SerializeField]
        private Image img_ladle;
        
        private Vector3 m_vr3_input;



        void Start()
        {
            
            
        }

        public virtual void OnDrag(PointerEventData pointerEventData)
        {
            Vector2 m_vt2_position;

           // pointerEventData.delta.magnitude;
            Vector3 screenPosition = new Vector3(pointerEventData.position.x, pointerEventData.position.y);
            Vector3 touchWorldCoordinates = Camera.main.ScreenToWorldPoint(screenPosition);
                if (img_cauldron.rectTransform.rect.Contains(touchWorldCoordinates))
                {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(img_cauldron.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out m_vt2_position);
                    //normalized input
                    m_vr3_input = new Vector3(m_vt2_position.x * 2 + 1, 0, m_vt2_position.y * 2 - 1);
                    m_vr3_input = m_vr3_input.normalized;

                    //ladle movement
                    img_ladle.rectTransform.anchoredPosition = new Vector3(m_vr3_input.x * (img_cauldron.rectTransform.sizeDelta.x / 3),
                                                                            m_vr3_input.z * (img_cauldron.rectTransform.sizeDelta.y / 3));
                //pointerEventData.delta.magnitude per i valori del mouse
                //img_ladle.transform.position = pointerEventData.delta.magnitude
            }

        }
        


        public virtual void OnPointerDown(PointerEventData pointerEventData)
        {
            OnDrag(pointerEventData);
        }
      

    }
}


