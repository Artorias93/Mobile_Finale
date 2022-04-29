using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NecromancerShop
{
    public class DraggableItem : MonoBehaviour
    {
        public bool b_IsIngredient
        {
            set { b_isIngredient = value; }
        }

        private Image image;
        private RectTransform rectTransform;
        private bool b_isDraggable=true;
        private Canvas m_baseCanvas;
        private Vector2 v_startPosition;
        private bool b_isIngredient = false;
        void Start()
        {
            m_baseCanvas = GameObject.FindGameObjectWithTag("MainUI").GetComponent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();


        }

        public void DragHandler(BaseEventData eventData)
        {
            if (b_isDraggable) {
                if (v_startPosition == Vector2.zero)
                {
                    v_startPosition = rectTransform.anchoredPosition;
                }
                PointerEventData pointerData = (PointerEventData)eventData;
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)m_baseCanvas.transform,
                    pointerData.position,
                    m_baseCanvas.worldCamera,
                    out position
                    );
                transform.position = m_baseCanvas.transform.TransformPoint(position);
            }
        }

        public void DropHandler(BaseEventData eventData)
        {
            if (v_startPosition != Vector2.zero)
            {
                rectTransform.anchoredPosition = v_startPosition;

            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (b_isIngredient && collision.gameObject.CompareTag("ShopCart"))
            {
                b_isDraggable = false;
                Color tmpColor = image.color;
                tmpColor.a = 0;
                image.color = tmpColor;
            }

        }
    }

}
